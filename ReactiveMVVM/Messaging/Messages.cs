using System;

namespace ReactiveMVVM.Messaging
{
    /// <summary>
    /// The message with sign name.
    /// </summary>
    public class SigneMessage : ISigneMessage
    {
        /// <summary>
        /// The name of the message
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the message's sender.
        /// </summary>
        public object Sender { get; set; }
    }

    /// <summary>
    /// The message with callback.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallbackMessage<T> : SigneMessage, ICallbackMessage<T>
    {
        /// <summary>
        /// call back action.
        /// </summary>
        public Action<T> Callback { get; set; }
    }



#if !WINDOWS_PHONE

    /// <summary>
    /// The query message.
    /// </summary>
    public class DynamicQueryMessage : CallbackMessage<dynamic>
    {
        /// <summary>
        /// The paramter can set any message sturct.
        /// </summary>
        public dynamic Paramter { get; set; }
    }

#endif



}
