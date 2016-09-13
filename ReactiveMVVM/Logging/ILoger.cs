using System;

namespace ReactiveMVVM.Logging
{


    /// <summary>
    /// The interface of logger. Allow you create youself logger.
    /// </summary>
    public interface ILogger
    {


        /// <summary>
        /// Log debug information. 
        /// </summary>
        /// <param name="message"></param>
        void Debug(object message);
        /// <summary>
        /// Log debug information. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Debug(object message, Exception exception);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void DebugFormat(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        void DebugFormat(string format, object arg0);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        void DebugFormat(string format, object arg0, object arg1);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void DebugFormat(string format, object arg0, object arg1, object arg2);


        void Info(object message);
        void Info(object message, Exception exception);
        void InfoFormat(string format, params object[] args);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);
        void InfoFormat(string format, object arg0);
        void InfoFormat(string format, object arg0, object arg1);
        void InfoFormat(string format, object arg0, object arg1, object arg2);

        void Warn(object message);
        void Warn(object message, Exception exception);
        void WarnFormat(string format, params object[] args);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
        void WarnFormat(string format, object arg0);
        void WarnFormat(string format, object arg0, object arg1);
        void WarnFormat(string format, object arg0, object arg1, object arg2);


        void Error(object message);
        void Error(object message, Exception exception);
        void ErrorFormat(string format, params object[] args);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);
        void ErrorFormat(string format, object arg0);
        void ErrorFormat(string format, object arg0, object arg1);
        void ErrorFormat(string format, object arg0, object arg1, object arg2);


        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void FatalFormat(string format, params object[] args);
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
        void FatalFormat(string format, object arg0);
        void FatalFormat(string format, object arg0, object arg1);
        void FatalFormat(string format, object arg0, object arg1, object arg2);


        /// <summary>
        /// Get or set the from which level will log message. 
        /// </summary>
        LogLevel LogLevel { get; set; }
    }



}
