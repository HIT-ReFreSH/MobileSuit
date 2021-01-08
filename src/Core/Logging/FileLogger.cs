using System;
using System.IO;

namespace PlasticMetal.MobileSuit.Core.Logging
{
    /// <summary>
    ///     Logger for MobileSuit.
    /// </summary>
    internal class FileLogger : ISuitLogger
    {
        /// <summary>
        ///     Initialize a File Logger with the log file path.
        /// </summary>
        /// <param name="path">Path to the log file.</param>
        protected internal FileLogger(string path)
        {
            Address = path;
            Writer =
                new StreamWriter(
                    new BufferedStream(
                        new FileStream(path, FileMode.OpenOrCreate,
                            FileAccess.Write, FileShare.Read)))
                {
                    AutoFlush = true
                };
        }


        /// <summary>
        ///     Writer of current log file
        /// </summary>
        protected TextWriter Writer { get; }

        /// <inheritdoc />
        public string Address { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Append(in LogEntry content)
        {
            try
            {
                Writer.WriteLine($"[{content.TimeStamp}]{content.Type}:{content.Message}");
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        ///     Close the writer stream
        /// </summary>
        ~FileLogger()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Release resources used
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            Writer.Flush();
            if (disposing) Writer.Dispose();
        }
    }
}