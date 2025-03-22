using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;

namespace NetApiCleanTemplate.WebApi.Logging;

public static class LoggerSetup
{
    private static readonly string LogConsoleTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
    private static readonly string LogFileTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{Tenant}/{UserName}] [{RequestPath}] {Message:lj}{NewLine}{Exception}";

    private static LoggerConfiguration ConfigSerilog(LoggerConfiguration? loggerConfiguration = null, ILogEventEnricher? enricher = null)
    {
        loggerConfiguration ??= new LoggerConfiguration();

        var conf = loggerConfiguration
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext();

        if (enricher != null)
        {
            conf = conf.Enrich.With(enricher);
        }

        conf = conf.WriteTo.Console(outputTemplate: LogConsoleTemplate, theme: AnsiConsoleTheme.Code)
            .WriteTo.File(
                "./Logs/log.txt",
                outputTemplate: LogFileTemplate,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 5000000, // 5 mb 
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30
            )
            .WriteTo.Logger(l =>
                l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                .WriteTo.File(
                    "./Logs/debug.txt",
                    outputTemplate: LogFileTemplate,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 5000000, // 5 mb 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 20
                )
            )
            .WriteTo.Logger(l =>
                l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File(
                    "./Logs/error.txt",
                    outputTemplate: LogFileTemplate,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 5000000, // 5 mb 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 20
                )
            )
            .WriteTo.Logger(l =>
                l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File(
                    "./Logs/info.txt",
                    outputTemplate: LogFileTemplate,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 5000000, // 5 mb 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 20
                )
            );

        return conf;
    }

    public static Serilog.ILogger GetSerilog()
    {
        return ConfigSerilog().CreateLogger();
    }
    public static IHostBuilder SetupSerilog(this IHostBuilder builder)
    {
        return builder.UseSerilog((HostBuilderContext hostBuilderContext, IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration) => {
            // var enricher = serviceProvider.GetService<LoggerEnricher>();
            ConfigSerilog(loggerConfiguration, null);
        });
    }
}
