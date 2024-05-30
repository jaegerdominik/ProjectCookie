using Microsoft.Extensions.Configuration;
using Serilog;
using utils.interfaces;

namespace utils;

public class AquariumLogger : IAquariumLogger
{
    private ILogger? _logger;

    public ILogger ContextLog<T>(string context) where T : class
    {
        // wait for the initialization of the logger
        if (this._logger == null)
        {
            this.Init().Wait();
        }

        if (this._logger == null)
        {
            throw new InvalidOperationException("Please initialize the Logger first by calling Init().");
        }

        return this._logger.ForContext(context, nameof(T));
    }

    public ILogger ContextLog<T>() where T : class
    {
        // default, if no context value is given
        return this.ContextLog<T>("Aquarium");
    }

    public async Task Init()
    {
        // initialize the logger using the given configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(
                "{\n\t\"Serilog\": {\n\t\t\"Using\": [\n\t\t\t\"Serilog.Sinks.Console\",\n\t\t\t\"Serilog.Sinks.File\",\n\t\t\t\"Serilog.Sinks.Seq\",\n\t\t\t\"Serilog.Enrichers.Environment\"\n\t\t],\n\t\t\"MinimumLevel\": {\n\t\t\t\"Default\": \"Verbose\",\n\t\t\t\"Override\": {\n\t\t\t\t\"Microsoft\": \"Warning\",\n\t\t\t\t\"System\": \"Warning\",\n\t\t\t\t\"Context.DataAcquisition\": \"Warning\"\n\t\t\t}\n\t\t},\n\t\t\"WriteTo\": [\n\t\t\t{\n\t\t\t\t\"Name\": \"Console\",\n\t\t\t\t\"Args\": {\n\t\t\t\t\t\"theme\": \"Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console\",\n\t\t\t\t\t\"outputTemplate\": \"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}\"\n\t\t\t\t}\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"Name\": \"File\",\n\t\t\t\t\"Args\": {\n\t\t\t\t\t\"path\": \"Logslog.log\",\n\t\t\t\t\t\"rollingInterval\": \"Day\"\n\t\t\t\t}\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"Name\": \"Seq\",\n\t\t\t\t\"Args\": {\n\t\t\t\t\t\"serverUrl\": \"http://127.0.0.1:5341\"\n\t\t\t\t}\n\t\t\t}\n\t\t],\n\t\t\"Enrich\": [\n\t\t\t\"FromLogContext\",\n\t\t\t\"WithMachineName\",\n\t\t\t\"WithThreadId\"\n\t\t]\n\t}\n}")))
            .Build();

        this._logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        await Task.CompletedTask;
        // could also be a synchronous function
        // return Task.CompletedTask;
    }
}