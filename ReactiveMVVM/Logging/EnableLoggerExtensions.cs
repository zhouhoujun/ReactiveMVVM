using System;
using System.Reactive.Linq;

namespace ReactiveMVVM.Logging
{
    /// <summary>
    /// Get the logger for Observable object.
    /// </summary>
    class LoggerForObservable : IEnableLogger 
    {

    }

    public static class EnableLoggerExtensions
    {
        
        readonly static ILogger mruLogger = new NullLogger();

        static TypeCache<int, ILogger> loggerCache =
           new TypeCache<int, ILogger>(
           (_, o) =>
           {
               var t = o.GetType();
               return LogManger.GetLogger(t.Namespace + "." + t.Name);
           }
                ,
           50);

        /// <summary>
        /// Returns the current logger object, which allows the object to
        /// log messages with the type name attached.
        /// </summary>
        /// <returns></returns>
        public static ILogger Logger(this IEnableLogger This)
        {
            // Prevent recursive meta-logging
            if (This is TypeCache<int, ILogger>)
                return mruLogger;

            lock (loggerCache)
            {
                return loggerCache.Get(This.GetHashCode(), This);
            }
        }
    }




    
}
