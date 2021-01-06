using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Parsing;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     A filter argument for Log finding
    /// </summary>
    public class LogFilter : AutoDynamicParameter
    {
        /// <summary>
        ///     Start time of logs to find
        /// </summary>
        [Option("s", 2)]
        [WithDefault]
        [SuitParser(typeof(LogFilter), "ParseDateTime")]
        public DateTime Start { get; set; } = DateTime.MinValue;

        /// <summary>
        ///     End time of logs to find
        /// </summary>
        [Option("e", 2)]
        [WithDefault]
        [SuitParser(typeof(LogFilter), "ParseDateTime")]
        public DateTime End { get; set; } = DateTime.MaxValue;

        /// <summary>
        ///     Type regex expression
        /// </summary>
        [Option("t")]
        [WithDefault]
        public string TypeRegex { get; set; } = string.Empty;

        /// <summary>
        ///     Message regex expression
        /// </summary>
        [Option("m")]
        [WithDefault]
        public string MessageRegex { get; set; } = string.Empty;

        /// <summary>
        ///     Parse DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object ParseDateTime(string s)
        {
            return DateTime.Parse(s, CultureInfo.CurrentCulture);
        }
    }

    /// <summary>
    ///     Log Manager
    /// </summary>
    [SuitInfo(typeof(LogRes), "Class")]
    public class LogDriver : SuitClient
    {
        private readonly Logger _logger;

        /**
         * Initialize a log driver with the logger
         *
         * [param logger the logger to operate
         */
        public LogDriver(Logger logger)
        {
            _logger = logger;
        }


        /// <summary>
        ///     Enable Run-time query
        /// </summary>
        /// <returns></returns>
        [SuitAlias("On")]
        [SuitInfo(typeof(LogRes), "Enable")]
        public TraceBack Enable()
        {
            _logger.EnableLogQuery = true;
            return TraceBack.AllOk;
        }

        /// <summary>
        ///     Disable Run-time query
        /// </summary>
        /// <returns></returns>
        [SuitAlias("Off")]
        [SuitInfo(typeof(LogRes), "Disable")]
        public TraceBack Disable()
        {
            _logger.EnableLogQuery = false;
            return TraceBack.AllOk;
        }

        /// <summary>
        ///     Find log with given filter
        /// </summary>
        /// <returns></returns>
        [SuitAlias("F")]
        [SuitInfo(typeof(LogRes), "Find")]
        public string Find(LogFilter filter)
        {
            var logsToShow =
                (from l in _logger.LogMem.AsParallel()
                    where l.TimeStamp >= filter.Start
                    where l.TimeStamp <= filter.End
                    where Regex.IsMatch(l.Type, filter.TypeRegex)
                    where Regex.IsMatch(l.Message, filter.MessageRegex)
                    orderby l.TimeStamp
                    select l).ToList();

            var i = 1;
            foreach (var e in logsToShow)
            {
                IO.WriteLine("(" + i + ") " + e.Type, OutputType.ListTitle);

                IO.AppendWriteLinePrefix();
                IO.WriteLine($"{LogRes.Time}: ");

                IO.AppendWriteLinePrefix();
                IO.WriteLine(e.TimeStamp.ToString("yyMMdd HH:mm:ss", CultureInfo.InvariantCulture),
                    OutputType.MobileSuitInfo);
                IO.SubtractWriteLinePrefix();

                IO.WriteLine($"{LogRes.Info}: ");

                IO.AppendWriteLinePrefix();
                var message = e.Message.Split("\n");
                foreach (var m in message) IO.WriteLine(m, OutputType.CustomInfo);
                IO.SubtractWriteLinePrefix();
                IO.SubtractWriteLinePrefix();
                i++;
            }

            return $"{logsToShow.Count}/{_logger.LogMem.Count}";
        }

        /// <summary>
        ///     Get Status of current log
        /// </summary>
        /// <returns></returns>
        [SuitAlias("S")]
        [SuitInfo(typeof(LogRes), "Status")]
        public string Status()
        {
            IO.WriteLine($"{LogRes.LogFileAt}: ");
            IO.AppendWriteLinePrefix();
            IO.WriteLine(_logger.FilePath, OutputType.MobileSuitInfo);
            IO.SubtractWriteLinePrefix();
            IO.WriteLine($"{LogRes.Dynamic}: ");
            IO.AppendWriteLinePrefix();
            IO.WriteLine(
                _logger.EnableLogQuery
                    ? LogRes.On
                    : LogRes.Off, OutputType.MobileSuitInfo);
            IO.SubtractWriteLinePrefix();
            return _logger.FilePath;
        }
    }
}