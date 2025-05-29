using m42Service.DSL;
using m42Service.Helpers;

namespace m42Service
{
    public class Worker : BackgroundService
    {
        private readonly TimersDSL _timersDSL;
        private readonly ConfigManager _configuration;
        public Worker(TimersDSL timersDSL,ConfigManager configuration)
        {
            File_Logger.GetLogFilePath_Event += () => _configuration.LogPath;
            _timersDSL = timersDSL;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timersDSL.Start(stoppingToken);
        }
    }
}
