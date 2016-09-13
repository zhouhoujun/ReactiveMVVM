using System;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM.Messaging
{

    #region property message interfaces

    /// <summary>
    /// The property message.
    /// </summary>
    public interface IPropertyMessages : ISenderMessage, IEnableLogger
    {
        /// <summary>
        /// Gets or sets the name of the property that changed.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets or sets the message's intended target. This property can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </summary>
        object Target { get; set; }
    }

    /// <summary>
    /// The property changed message.
    /// </summary>
    /// <typeparam name="T">the type of property value</typeparam>
    public interface IPropertyChangedMessage<T> : IPropertyMessages, IEnableLogger
    {
        /// <summary>
        /// Gets the value that the property has after the change.
        /// </summary>
        T NewValue { get; }

        /// <summary>
        /// Gets the value that the property had before the change.
        /// </summary>
        T OldValue { get; }
    }

    /// <summary>
    /// property changed message. with callback function, need regist in 
    /// </summary>
    /// <typeparam name="T">the type of property value</typeparam>
    public interface IPropertyChangedMessageWithCallback<T> : IPropertyChangedMessage<T>, IEnableLogger
    {
        /// <summary>
        /// the call back action
        /// </summary>
        Action<T> Callback { get; }
    }

    #endregion
}
