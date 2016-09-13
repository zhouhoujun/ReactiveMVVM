using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM
{
    /// <summary>
    /// The interface of IObservableObject, it can send message.
    /// </summary>
    public interface IObservableObject : IObservable<object>, IDisposable, IEnableLogger
    {

        /// <summary>
        /// send the message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="wait">wait some milliseconds to send the message. </param>
        /// <returns>return the disposable, if dispose it means to cancel the send.</returns>
        IDisposable Send<T>(T message, int wait = 0);

        /// <summary>
        /// send the message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="onError">To do deal with errors when send message.</param>
        /// <param name="wait">wait some milliseconds to send the message. </param>
        /// <returns>return the disposable, if dispose it means to cancel the send.</returns>
        IDisposable Send<T>(T message, Action<Exception> onError, int wait = 0);

        /// <summary>
        /// send the message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="onCompleted">When send message completed to do the onCompleted action.</param> 
        /// <param name="wait">wait some milliseconds to send the message. </param>
        /// <returns>return the disposable, if dispose it means to cancel the send.</returns>
        IDisposable Send<T>(T message, Action onCompleted, int wait = 0);

        /// <summary>
        /// send the message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="onError">To do deal with errors when send message.</param>
        /// <param name="onCompleted">When send message completed to do the onCompleted action.</param> 
        /// <param name="wait">wait some milliseconds to send the message. </param>
        /// <returns>return the disposable, if dispose it means to cancel the send.</returns>
        IDisposable Send<T>(T message, Action<Exception> onError, Action onCompleted, int wait = 0);

    }
}
