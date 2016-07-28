using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System.IO;

namespace Iquality.Shared.OutboxMailer.Core.Logging
{
    public static class SerilogConfigurator
    {
        public static void Configure(IHostingEnvironment env)
        {
            //.Filter.ByIncludingOnly(x => x.Level == LogEventLevel.Information)

            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss} {StandardLengthLevelName} {SourceContext}:{NewLine}{Tab}{Message}{NewLine}{Exception}";

            var projRoot = Path.GetDirectoryName(env.WebRootPath);
            var solutionRoot = Path.GetDirectoryName(projRoot);
            // var solutionRoot = Path.GetDirectoryName(projRoot);

            var verboseLogger = new LoggerConfiguration()
               .Filter.ByIncludingOnly(x =>
                    x.Level == LogEventLevel.Verbose
                    || x.Level == LogEventLevel.Debug)
               .WriteTo.RollingFile(
                   solutionRoot + @"\Logs\Api\verbose\log-{Date}.txt",
                   outputTemplate: template,
                   retainedFileCountLimit: 5
               )
             .CreateLogger();

            var errorLogger = new LoggerConfiguration()
             .Filter.ByIncludingOnly(x =>
                x.Level == LogEventLevel.Error
                || x.Level == LogEventLevel.Fatal
                || x.Level == LogEventLevel.Warning)
             .WriteTo.RollingFile(
                 solutionRoot + @"\Logs\Api\error\log-{Date}.txt",
                 outputTemplate: template,
                 retainedFileCountLimit: 5
             )
           .CreateLogger();

            var informationLogger = new LoggerConfiguration()
              .Filter.ByIncludingOnly(x => x.Level == LogEventLevel.Information)
              .WriteTo.RollingFile(
                  solutionRoot + @"\Logs\Api\info\log-{Date}.txt",
                  outputTemplate: template,
                  retainedFileCountLimit: 5
              )
            .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Logger(errorLogger)
                .WriteTo.Logger(informationLogger)
                .WriteTo.Logger(verboseLogger)
                .Enrich.FromLogContext().Enrich.With<StandardLengthLevelName>()
                .CreateLogger();
        }
    }
}