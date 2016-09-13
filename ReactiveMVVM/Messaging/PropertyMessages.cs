using System;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM.Messaging
{
    #region property messages

    public class PropertyMessage : IPropertyMessages
    {
        /// <summary>
        /// Gets or sets the message's sender.
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that changed.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the message's intended target. This property can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </summary>
        public object Target { get; set; }
    }

    /// <summary>
    /// The property changed message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyChangedMessage<T> : PropertyMessage, IPropertyChangedMessage<T>
    {

        /// <summary>
        /// Gets the value that the property has after the change.
        /// </summary>
        public T NewValue { get; set; }

        /// <summary>
        /// Gets the value that the property had before the change.
        /// </summary>
        public T OldValue { get; set; }

    }

    public class PropertyChangedMessageWithCallback<T> : PropertyChangedMessage<T>, IPropertyChangedMessageWithCallback<T>
    {
        /// <summary>
        /// call back action.
        /// </summary>
        public Action<T> Callback { get; set; }
    }

    #endregion
}
