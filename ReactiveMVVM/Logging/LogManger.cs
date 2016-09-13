using System;

namespace ReactiveMVVM.Logging
{
    /// <summary>
    /// The log manager to setup the logger.
    /// </summary>
    public static class LogManger
    {
        /// <summary>
        /// the logger factory. Default value is DefaultLogger constructor.
        /// </summary>
        public static Func<string, ILogger> LoggerFactory { get; set; }

        /// <summary>
        /// Get or set the from which level can log message. 
        /// </summary>
        public static LogLevel LogLevel { get; set; }

        static LogManger()
        {
            LoggerFactory= (x => new DefaultLogger());
            LogLevel = Logging.LogLevel.Info;
        }

        /// <summary>
        /// Get the logger with friendly name.
        /// </summary>
        /// <param name="name">the friendly name of the loggger.</param>
        /// <returns></returns>
        public static ILogger GetLogger(string name)
        {
            var logger= LoggerFactory(name);
            logger.LogLevel= LogLevel;
            return logger;
        }
    }
}
