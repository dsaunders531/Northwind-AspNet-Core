using mezzanine.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace mezzanine.Utility
{
    /// <summary>
    /// The Logger which does the work of saving the log data.
    /// </summary>
    public sealed class XMLLogger : ILogger, IDisposable
    {
        private LogLevel LogLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.None;
        private string CategoryName { get; set; } = string.Empty; // Not sure what this is for but its in the interface...
        private string OutputPath { get; set; } = string.Empty;
        private XmlDocument LogDocument { get; set; } = null;
        private bool Busy { get; set; } = false;
        private long MaxLogRows { get; set; } = 1000;

        public XMLLogger(LogLevel logLevel, string outputPath)
        {
            LogLevel = logLevel;
            OutputPath = outputPath;
        }

        public XMLLogger(LogLevel logLevel, string categoryName, string outputPath)
        {
            LogLevel = logLevel;
            CategoryName = categoryName;
            OutputPath = outputPath;
        }

        public XMLLogger(LogLevel logLevel, string categoryName, string outputPath, long rotateWhenRows)
        {
            LogLevel = logLevel;
            CategoryName = categoryName;
            OutputPath = outputPath;
            MaxLogRows = rotateWhenRows;
        }

        public XMLLogger(LogLevel logLevel, string outputPath, long rotateWhenRows)
        {
            LogLevel = logLevel;
            OutputPath = outputPath;
            MaxLogRows = rotateWhenRows;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            // There is no scope.
            return null;
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return LogLevel == logLevel;
        }

        /// <summary>
        /// Write the XML log entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            XmlElement xEleNew = null;
            XmlNode xLogEntries = null;
            XmlAttribute xa = null;
            long rowCount = 0;

            // only log at the specified level and below
            if ((int)this.LogLevel <= (int)logLevel && logLevel != LogLevel.None)
            {
                // Make sure requests do not bump into each other.
                while (Busy == true)
                {
                    System.Threading.Thread.Sleep(250);
                }

                Busy = true;

                try
                {
                    LogDocument = this.LoadOrCreateLogDocument(OutputPath);
                    // Get row information from file.
                    xLogEntries = (from XmlNode xn in LogDocument.ChildNodes where xn.Name == "logEntries" select xn).First();
                    rowCount = Convert.ToInt64(xLogEntries.Attributes.GetNamedItem(@"entryCount").Value);

                    if (rowCount > MaxLogRows)
                    {
                        this.LogRotate();

                        // get the important info again.
                        xLogEntries = null;
                        xLogEntries = (from XmlNode xn in LogDocument.ChildNodes where xn.Name == "logEntries" select xn).First();
                        rowCount = Convert.ToInt64(xLogEntries.Attributes.GetNamedItem(@"entryCount").Value);
                    }

                    // Write the element items
                    xEleNew = LogDocument.CreateElement(@"logEntry");
                    xa = LogDocument.CreateAttribute("id");
                    xa.Value = rowCount.ToString();
                    xEleNew.Attributes.Append(xa);

                    // Create the object data and serialise it.
                    LogEntryData le = this.Create_EntryData(logLevel, eventId, state, exception, formatter);

                    using (XMLSerializer serializer = new XMLSerializer())
                    {
                        xEleNew.InnerXml += serializer.Serialize(le.GetType(), le, @"logEntry").InnerXml;
                    }

                    rowCount++;

                    xLogEntries.Attributes.GetNamedItem(@"entryCount").Value = rowCount.ToString();
                    xLogEntries.AppendChild(xEleNew);

                    try
                    {
                        using (FileStream fs = new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            LogDocument.Save(fs);
                            // Cleardown items to prevent file lock.
                            fs.Flush();
                            fs.Dispose();
                        }
                    }
                    finally
                    {
                        LogDocument = null;
                    }
                }
                catch (Exception ex)
                {
                    // TODO own exceptions are being logged!
                    // throw new LoggerException(@"Something went wrong while logging using " + this.GetType().ToString() + " see inner exception for details.", ex);
                }
                finally
                {
                    Busy = false;
                    xEleNew = null;
                    xLogEntries = null;
                    xa = null;
                }
            }
        }

        private LogEntryData Create_EntryData<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogEntryData result = new LogEntryData()
            {
                LogLevel = logLevel.ToString(),
                EventId = eventId.Id.ToString(),
                EventName = eventId.Name,
                Timestamp = DateTime.UtcNow
            };

            if (state != null)
            {
                result.State = state.ToString();
            }

            if (exception != null)
            {
                result.Exception = this.Create_ExceptionData(exception);
            }

            return result;
        }

        private LogExceptionData Create_ExceptionData(Exception ex)
        {
            LogExceptionData result = new LogExceptionData()
            {
                ExceptionType = ex.GetType().ToString(),
                HelpLink = ex.HelpLink,
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace
            };

            if (ex.InnerException != null)
            {
                result.InnerException = this.Create_ExceptionData(ex.InnerException);
            }

            return result;
        }

        private XmlDocument LoadOrCreateLogDocument(string path)
        {
            XmlDocument result = new XmlDocument();

            if (File.Exists(path) == false)
            {
                // Create new file.
                XmlElement xe;
                XmlAttribute xa;
                XmlProcessingInstruction xpi = result.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");

                xe = result.CreateElement("logEntries");
                xa = result.CreateAttribute("entryCount");
                xa.Value = @"0";

                xe.Attributes.Append(xa);
                result.AppendChild(xpi);
                result.AppendChild(xe);

                xe = null;
                xa = null;
                xpi = null;
            }
            else
            {
                // Load the file.
                XmlReaderSettings xrSett = new XmlReaderSettings { CloseInput = true, ConformanceLevel = ConformanceLevel.Document };

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader xr = XmlReader.Create(fs, xrSett))
                    {
                        result.Load(xr);
                        xr.Close();
                    }
                }

                xrSett = null;
            }

            return result;
        }

        private string Create_NewLogPath(byte logCounter = 1)
        {
            string result = OutputPath;
            FileInfo fInf = new FileInfo(result);
            string newFileName = fInf.Name.Replace(fInf.Extension, string.Empty);

            if (newFileName.Contains(@"_"))
            {
                // avoid name_1_1 problems
                newFileName = newFileName.Substring(0, newFileName.IndexOf(@"_"));
            }

            result = fInf.DirectoryName + @"\" + newFileName + @"_" + logCounter.ToString() + fInf.Extension;

            return result;
        }

        /// <summary>
        /// Move the logs around.
        /// </summary>
        private void LogRotate()
        {
            string newPath = OutputPath;
            byte logCounter = 1;

            newPath = this.Create_NewLogPath(logCounter);
            while (File.Exists(newPath) == true && logCounter <= 255)
            {
                logCounter++;
                newPath = this.Create_NewLogPath(logCounter);
            }

            if (logCounter >= 255)
            {
                throw new LoggerException(@"Too many log files! 255 files are supported.");
            }

            this.OutputPath = newPath;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    try
                    {
                        // Attempt to save the log file
                        if (LogDocument != null && OutputPath != string.Empty)
                        {
                            using (FileStream fs = new FileStream(OutputPath, FileMode.Create))
                            {
                                LogDocument.Save(fs);
                                // Cleardown items to prevent file lock.
                                fs.Flush();
                                fs.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        LogDocument = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~XMLLogger() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
