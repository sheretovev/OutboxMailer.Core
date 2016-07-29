using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using System.Threading;


namespace Iquality.Shared.OutboxMailer.Core.Tasks
{

    public class ScheduledSender
    {
        public static void Startup()
        {
            var singularity = Singularity.Instance;

            var job = new SimpleParameterizedJob<string>(Execute);

            var schedule = new EveryXTimeSchedule(TimeSpan.FromSeconds(1));

            var scheduledJob = singularity.ScheduleParameterizedJob(
                schedule, job, "Hello World", true); //starts immediately

            var startTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(5));

            var scheduledJob2 = singularity.ScheduleParameterizedJob(
                schedule, job, "Hello World 2", startTime);

            singularity.Start();

            Thread.Sleep(10 * 1000);

            singularity.StopScheduledJob(scheduledJob);

            Thread.Sleep(5 * 1000);

            singularity.Stop();
        }

        public static void Execute(string parameter, DateTime scheduledTime)
        {
            var loggerFactory = DependencyResolver.Services.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<ScheduledSender>();
            logger.LogError($"{parameter}\tscheduled: {scheduledTime.ToString("o")}");
        }
    }


    //public class OutboxSenderTask : Job
    //{

    //    public OutboxSenderTask(TimeSpan interval, TimeSpan timeout)
    //        : base("Sample Job", interval, timeout)
    //        {
    //        }

    //        public override Task Execute()
    //        {
    //        System.Threading.
    //            return new Task(() => Thread.Sleep(3000));
    //        }
     
    //}
}

