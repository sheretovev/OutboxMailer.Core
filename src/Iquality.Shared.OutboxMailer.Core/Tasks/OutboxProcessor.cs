using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using Iquality.Shared.OutboxMailer.Core.Extensions;
using Iquality.Shared.OutboxMailer.Core.Models;
using System.Linq;
using Iquality.Shared.OutboxMailer.Core.Mailer;

namespace Iquality.Shared.OutboxMailer.Core.Tasks
{
    public class OutboxProcessorSettings
    {
        public int ItemsPerShot { get; set; }
        public TimeSpan Frequency { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }

    public class OutboxProcessor
    {
        public static void Startup(OutboxProcessorSettings settings)
        {   
            var logger = DependencyResolver.Services.GetService<ILoggerFactory>().CreateLogger<OutboxProcessor>();
            logger.LogInformation($"Scheduled Task is started. Settings: {settings.ToLog()}");

            var singularity = Singularity.Instance;
            var job = new SimpleParameterizedJob<OutboxProcessorSettings>(Execute);

            var schedule = new EveryXTimeSchedule(settings.Frequency);
                        
            var scheduledJob = singularity.ScheduleParameterizedJob(
                schedule, job, settings, settings.StartTime);
            
            singularity.Start();
        }

        public static void Execute(OutboxProcessorSettings parameter, DateTime scheduledTime)
        {
            var logger = DependencyResolver.Services.GetService<ILoggerFactory>().CreateLogger<OutboxProcessor>();
            logger.LogInformation($"{parameter}\tscheduled: {scheduledTime.ToString("o")}");            
            var sender = new SmtpEmailSender(logger);
            // find uprocessed email and process them
            var unprocessedMessages = OutboxContext.RunInDb(context => context.Set<OutboxMessage>().Where(message => message.Status == 0).ToList());
            foreach (var message in unprocessedMessages)
            {
                try
                {
                    OutboxContext.RunInDb(context => AssignStatus(context, message.OutboxMessageId, ProcessStatus.Busy));
                    sender.Send(message.ToAddress, message.FromAddress, message.Subject, message.Body);
                    OutboxContext.RunInDb(context => AssignStatus(context, message.OutboxMessageId, ProcessStatus.Sent));
                }
                catch (Exception ex)
                {
                    logger.LogError($"Sending failed. email: {message.ToLog()}", ex);
                    OutboxContext.RunInDb(context => AssignStatus(context, message.OutboxMessageId, ProcessStatus.Error));
                }
            }
        }

        private static void AssignStatus(Microsoft.EntityFrameworkCore.DbContext context, Guid id, ProcessStatus status)
        {
            // todo: consider case when several tasks works with same table and item is no longer relevant to process.
            // check status before assign new status
            var a = context.Set<OutboxMessage>().FirstOrDefault(x => x.OutboxMessageId == id);
            a.ProcessedDate = DateTime.UtcNow;
            a.Status = status;
        }
    }
}