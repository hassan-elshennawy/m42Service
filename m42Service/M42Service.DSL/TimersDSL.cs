using m42Service.Helpers;
using M42Service.DSL;
using Timer = System.Timers.Timer;

namespace m42Service.DSL
{
    public class TimersDSL
    {
        private readonly List<Timer> timersList = new List<Timer>();
        private readonly File_Logger _logger;
        private readonly ConfigManager _configuration;
        private readonly M42Dsl _M42Dsl;

        public TimersDSL(ConfigManager configuration, M42Dsl m42)
        {
            _logger = File_Logger.GetInstance("TimersDSL");
            _M42Dsl = m42;
            _configuration = configuration;
        }

        public void Start(CancellationToken stoppingToken)
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Worker start running at: {DateTimeOffset.Now}");

            if (timersList.Any())
            {
                StopTimers();
            }

            // Initialize timers for each process
            timersList.Add(InitTimer(async () => await _M42Dsl.processAuthReq(),_configuration.LdmStartTime,_configuration.IntervalTime));
            timersList.Add(InitTimer(async () => await _M42Dsl.processUpdatehReq(), _configuration.LdmStartTime, _configuration.IntervalTime));

        }

        private Timer InitTimer(Action timerProcess, int startAfterSeconds, double intervalInSeconds)
        {
             var timer = new Timer()
            {
                Enabled = true,
                Interval = startAfterSeconds * 1000
            };

            timer.Elapsed += (sender, e) =>
            {
                try
                {
                    timer.Enabled = false;
                    timerProcess();
                }
                catch (Exception ex)
                {
                    _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception in timer process: {ex}");
                }
                finally
                {
                    timer.Interval = intervalInSeconds * 1000;
                    timer.Enabled = true;
                }
            };

            return timer;
        }

        public void StopTimers()
        {
            try
            {
                foreach (var timer in timersList)
                {
                    Stop(timer);
                }
                timersList.Clear();
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception stopping timers: {ex}");
            }
        }

        private void Stop(Timer timer)
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

    }
}