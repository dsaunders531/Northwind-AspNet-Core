using mezzanine.Utility;
using Microsoft.Extensions.Logging;
using System;

namespace mezzanine.Services
{
    /// <summary>
    /// Provide logging to XML file
    /// </summary>
    public class XMLLoggerProvider : Microsoft.Extensions.Logging.ILoggerProvider
    {
        private LogLevel LogLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.None;
        private string OutputPath { get; set; } = string.Empty;
        private XMLLogger Logger { get; set; } = null;
        private long DefaultMaxRows { get; set; } = 1000;

        public XMLLoggerProvider(LogLevel logLevel, string outputPath, long rotateWhenRows)
        {
            LogLevel = logLevel;
            OutputPath = outputPath;
            DefaultMaxRows = rotateWhenRows;
        } 

        ILogger ILoggerProvider.CreateLogger(string categoryName)
        {
            // Only 1 instance of logger per logger provider.
            if (Logger == null)
            {
                Logger = new XMLLogger(LogLevel, categoryName, OutputPath,DefaultMaxRows);
            }
            return Logger;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Logger.Dispose();
                    Logger = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~XMLLoggerProvider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
