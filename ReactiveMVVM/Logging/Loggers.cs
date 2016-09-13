using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ReactiveMVVM.Logging
{

    /// <summary>
    /// The null object of logger. It do nothing.
    /// </summary>
    public class NullLogger : ILogger
    {
        public NullLogger(string _ = null) { }

        public void Debug(object message) { }
        public void Debug(object message, Exception exception) { }
        public void DebugFormat(string format, params object[] args) { }
        public void DebugFormat(IFormatProvider provider, string format, params object[] args) { }
        public void DebugFormat(string format, object arg0) { }
        public void DebugFormat(string format, object arg0, object arg1) { }
        public void DebugFormat(string format, object arg0, object arg1, object arg2) { }

        public void Info(object message) { }
        public void Info(object message, Exception exception) { }
        public void InfoFormat(string format, params object[] args) { }
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) { }
        public void InfoFormat(string format, object arg0) { }
        public void InfoFormat(string format, object arg0, object arg1) { }
        public void InfoFormat(string format, object arg0, object arg1, object arg2) { }


        public void Warn(object message) { }
        public void Warn(object message, Exception exception) { }
        public void WarnFormat(string format, params object[] args) { }
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { }
        public void WarnFormat(string format, object arg0) { }
        public void WarnFormat(string format, object arg0, object arg1) { }
        public void WarnFormat(string format, object arg0, object arg1, object arg2) { }

        public void Error(object message) { }
        public void Error(object message, Exception exception) { }
        public void ErrorFormat(string format, params object[] args) { }
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { }
        public void ErrorFormat(string format, object arg0) { }
        public void ErrorFormat(string format, object arg0, object arg1) { }
        public void ErrorFormat(string format, object arg0, object arg1, object arg2) { }

        public void Fatal(object message) { }
        public void Fatal(object message, Exception exception) { }
        public void FatalFormat(string format, params object[] args) { }
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) { }
        public void FatalFormat(string format, object arg0) { }
        public void FatalFormat(string format, object arg0, object arg1) { }
        public void FatalFormat(string format, object arg0, object arg1, object arg2) { }


        /// <summary>
        /// Get or set the from which level will log message. 
        /// </summary>
        public LogLevel LogLevel { get; set; }
    }

    /// <summary>
    /// The logger base class.
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        string _prefix;
        string _prefixBuffer = "";
        DateTime _lastUpdated;
        /// <summary>
        /// the tiny interval time to log message.
        /// </summary>
        readonly TimeSpan _LogInterval = TimeSpan.FromMilliseconds(50.0);
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix">the logger's friendly name</param>
        public LoggerBase(string prefix = null)
        {
            this._prefix = prefix;

            _lastUpdated = DateTime.MinValue;
        }

        /// <summary>
        /// To deal with the debug message. 
        /// </summary>
        /// <param name="message">debug string message</param>
        protected abstract void LogDebug(string message);

        /// <summary>
        /// To deal with the information message.
        /// </summary>
        /// <param name="message"></param>
        protected abstract void LogInfo(string message);

        /// <summary>
        /// To deal with the warn message.
        /// </summary>
        /// <param name="message"></param>
        protected abstract void LogWarn(string message);

        /// <summary>
        /// To deal with the error message.
        /// </summary>
        /// <param name="message"></param>
        protected abstract void LogError(string message);

        /// <summary>
        /// To deal with the fatal message.
        /// </summary>
        /// <param name="message"></param>
        protected abstract void LogFatal(string message);

        /// <summary>
        /// Get or set the from which level will log message. 
        /// </summary>
        public LogLevel LogLevel { get; set; }


        public void Debug(object message) { Translate(LogDebug, message); }
        public void Debug(object message, Exception exception) { Translate(LogDebug, message, exception); }
        public void DebugFormat(string format, params object[] args) { Translate(LogDebug, format, args, null); }
        public void DebugFormat(IFormatProvider provider, string format, params object[] args) { Translate(LogDebug, format, args, provider); }
        public void DebugFormat(string format, object arg0) { Translate(LogDebug, format, arg0); }
        public void DebugFormat(string format, object arg0, object arg1) { Translate(LogDebug, format, arg0, arg1); }
        public void DebugFormat(string format, object arg0, object arg1, object arg2) { Translate(LogDebug, format, arg0, arg1, arg2); }

        public void Info(object message) { Translate(LogInfo, message); }
        public void Info(object message, Exception exception) { Translate(LogInfo, message, exception); }
        public void InfoFormat(string format, params object[] args) { Translate(LogInfo, format, args, null); }
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) { Translate(LogInfo, format, args, provider); }
        public void InfoFormat(string format, object arg0) { Translate(LogInfo, format, arg0); }
        public void InfoFormat(string format, object arg0, object arg1) { Translate(LogInfo, format, arg0, arg1); }
        public void InfoFormat(string format, object arg0, object arg1, object arg2) { Translate(LogInfo, format, arg0, arg1, arg2); }

        public void Warn(object message) { Translate(LogWarn, message); }
        public void Warn(object message, Exception exception) { Translate(LogWarn, message, exception); }
        public void WarnFormat(string format, params object[] args) { Translate(LogWarn, format, args, null); }
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { Translate(LogWarn, format, args, provider); }
        public void WarnFormat(string format, object arg0) { Translate(LogWarn, format, arg0); }
        public void WarnFormat(string format, object arg0, object arg1) { Translate(LogWarn, format, arg0, arg1); }
        public void WarnFormat(string format, object arg0, object arg1, object arg2) { Translate(LogWarn, format, arg0, arg1, arg2); }

        public void Error(object message) { Translate(LogError, message); }
        public void Error(object message, Exception exception) { Translate(LogError, message, exception); }
        public void ErrorFormat(string format, params object[] args) { Translate(LogError, format, args, null); }
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { Translate(LogError, format, args, provider); }
        public void ErrorFormat(string format, object arg0) { Translate(LogError, format, arg0); }
        public void ErrorFormat(string format, object arg0, object arg1) { Translate(LogError, format, arg0, arg1); }
        public void ErrorFormat(string format, object arg0, object arg1, object arg2) { Translate(LogError, format, arg0, arg1, arg2); }
        
        public void Fatal(object message) { Translate(LogFatal, message); }
        public void Fatal(object message, Exception exception) { Translate(LogFatal, message, exception); }
        public void FatalFormat(string format, params object[] args) { Translate(LogFatal, format, args, null); }
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) { Translate(LogFatal, format, args, provider); }
        public void FatalFormat(string format, object arg0) { Translate(LogFatal, format, arg0); }
        public void FatalFormat(string format, object arg0, object arg1) { Translate(LogFatal, format, arg0, arg1); }
        public void FatalFormat(string format, object arg0, object arg1, object arg2) { Translate(LogFatal, format, arg0, arg1, arg2); }


        /// <summary>
        /// if can write this level log.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected bool CanWrite(LogLevel level)
        {
            return ((int)level >= (int)LogLevel);
        }

        #region Translate log message

        private string GetLogPrefix()
        {
            if (DateTime.Now - _lastUpdated < _LogInterval)
            {
                return _prefixBuffer;
            }

            var now = DateTime.Now;
            var buffer = new StringBuilder(128);

            buffer.Append(now.ToString());
            buffer.AppendFormat(" [{0}] ", Thread.CurrentThread.ManagedThreadId);
            buffer.Append(_prefix);
            buffer.Append(": ");
            _prefixBuffer = buffer.ToString();

            _lastUpdated = now;
            return buffer.ToString();
        }

        private void Translate(Action<string> channel, object message)
        {
            channel(GetLogPrefix() + message.ToString());
        }

        private void Translate(Action<string> channel, object message, Exception exception)
        {
            channel(GetLogPrefix() + String.Format("{0}: {1}\n{2}", message, exception.Message, exception));
        }

        private void Translate(Action<string> channel, string format, object arg0)
        {
            Contract.Requires(format != null);
            channel(GetLogPrefix() + String.Format(format, arg0));
        }

        private void Translate(Action<string> channel, string format, object arg0, object arg1)
        {
            Contract.Requires(format != null);
            channel(GetLogPrefix() + String.Format(format, arg0, arg1));
        }

        private void Translate(Action<string> channel, string format, object arg0, object arg1, object arg2)
        {
            Contract.Requires(format != null);
            channel(GetLogPrefix() + String.Format(format, arg0, arg1, arg2));
        }

        private void Translate(Action<string> channel, string format, object[] args, IFormatProvider provider = null)
        {
            Contract.Requires(format != null);
            provider = provider ?? (IFormatProvider)CultureInfo.InvariantCulture;
            channel(GetLogPrefix() + String.Format(format, args, provider));
        }


        #endregion

    }

    /// <summary>
    /// The default logger just to wirte message in cosole .
    /// </summary>
    public class DefaultLogger : LoggerBase
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix">the logger's friendly name</param>
        public DefaultLogger(string prefix = null)
            : base(prefix) { }

        static object gate = 1;

        protected override void LogDebug(string message)
        {
            if (!CanWrite(LogLevel.Debug))
                return;

            lock (gate) { Console.WriteLine("Debug: " + message); }
        }

        protected override void LogWarn(string message)
        {
            if (!CanWrite(LogLevel.Warn))
                return;

            lock (gate) { Console.WriteLine("Warn: " + message); }
        }

        protected override void LogInfo(string message)
        {
            if (!CanWrite(LogLevel.Info))
                return;

            lock (gate) { Console.WriteLine("Info: " + message); }
        }

        protected override void LogError(string message)
        {
            if (!CanWrite(LogLevel.Error))
                return;

            lock (gate) { Console.WriteLine("ERROR: " + message); }
        }

        protected override void LogFatal(string message)
        {
            lock (gate) { Console.WriteLine("FATAL ERROR!!!: ***" + message + " ***"); }
        }
    }

}
