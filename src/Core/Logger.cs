using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{


    /// <summary>
    /// Logger for MobileSuit
    /// </summary>
    public class Logger : ILogger, IDisposable
    {
        /// <summary>
        /// Record of memorized logs.
        /// </summary>
        public List<LogEntry> LogMem { get; } = new List<LogEntry>();
        /// <summary>
        /// Whether The log can be queried at run time driver or not.
        /// </summary>
        public bool EnableLogQuery { get; set; }




        /// <summary>
        /// Close the writer stream
        /// </summary>
        ~Logger()
        {
            Dispose(false);
        }

        private void WriteLog(string info, string content)
        {
            var time = DateTime.Now;
            var output = $"[{time}]{info}:{content}\n";
            if (EnableLogQuery)
            {
                LogMem.Add(new LogEntry(time, info, content));
            }
            try
            {
                Writer.Write(output);
                Writer.Flush();

            }
            catch (IOException)
            {
            }
        }

        private async Task WriteLogAsync(string info, string content)
        {
            var time = DateTime.Now;
            var output = $"[{time}]{info}:{content}\n";
            if (EnableLogQuery)
            {
                LogMem.Add(new LogEntry(time, info, content));
            }
            try
            {
                await Writer.WriteAsync(output).ConfigureAwait(false);
                await Writer.FlushAsync().ConfigureAwait(false);

            }
            catch (IOException)
            {
            }
        }

        /// <inheritdoc />
        public void LogDebug(string content)
        {
            WriteLog("Info", content);
        }

        /// <inheritdoc />
        public void LogCommand(string content)
        {
            WriteLog("Command", content);
        }


        /// <inheritdoc />
        public void LogTraceBack(TraceBack content, object? returnValue = null)
        {
            WriteLog("TraceBack", content + returnValue switch
            {
                { } => $"({returnValue})",
                null => ""
            });
        }

        /// <inheritdoc />
        public void LogException(string content)
        {
            WriteLog("Exception", content);
        }

        /// <inheritdoc />
        public void LogException(Exception content)
        {
            if (content == null) return;
            StringBuilder stringBuilder = new StringBuilder(content.Message);
            foreach (var se in new StackTrace(content).GetFrames())
            {
                stringBuilder.Append("\n\tAt ").Append(se);
            }
            WriteLog(content.GetType().Name, stringBuilder.ToString());
        }

        /// <inheritdoc />
        public Task LogDebugAsync(string content)
        => WriteLogAsync("Info", content);


        /// <inheritdoc />
        public Task LogCommandAsync(string content)
        => WriteLogAsync("Command", content);



        /// <inheritdoc />
        public Task LogTraceBackAsync(TraceBack content, object? returnValue = null)
        => WriteLogAsync("TraceBack", content + returnValue switch
        {
            { } => $"({returnValue})",
            null => ""
        });

        /// <inheritdoc />
        public Task LogExceptionAsync(string content)
        => WriteLogAsync("Exception", content);

        /// <inheritdoc />
        public Task LogExceptionAsync(Exception content)
        {
            if (content == null) return Task.Run(() => { });
            StringBuilder stringBuilder = new StringBuilder(content.Message);
            foreach (var se in new StackTrace(content).GetFrames())
            {
                stringBuilder.Append("\n\tAt ").Append(se);
            }
            return WriteLogAsync(content.GetType().Name, stringBuilder.ToString());
        }
        /// <inheritdoc />
        public string FilePath { get; }

        /// <summary>
        /// Writer of current log file
        /// </summary>
        protected TextWriter Writer { get; }

        /// <summary>
        /// Initialize a SuitLogger with the log file path
        /// </summary>
        /// <param name="path"></param>
        protected internal Logger(string path)
        {
            FilePath = path;
            Writer =
                new StreamWriter(
                new BufferedStream(
                    new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)));


        }


        /// <summary>
        /// Release resources used
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Writer.Dispose();
            }
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
