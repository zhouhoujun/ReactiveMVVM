
using System;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM.Messaging
{

    /// <summary>
    /// The message base infterface.
    /// </summary>
    public interface ISenderMessage : IMessage, IEnableLogger
    {
        /// <summary>
        /// Gets or sets the message's sender.
        /// </summary>
        object Sender { get; }
    }

    /// <summary>
    /// The message with sign name.
    /// </summary>
    public interface ISigneMessage : ISenderMessage, IEnableLogger
    {
        /// <summary>
        /// the name of the message
        /// </summary>
        string Name { get; }
    }


    /// <summary>
    /// The message with name and call back function.
    /// </summary>
    public interface ICallbackMessage<T> : ISigneMessage
    {
        /// <summary>
        /// the call back action
        /// </summary>
        Action<T> Callback { get; }
    }



}
