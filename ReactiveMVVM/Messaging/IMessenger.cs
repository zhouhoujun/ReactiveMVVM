using System;
using System.Reactive.Concurrency;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM.Messaging
{
    /// <summary>
    /// The messager, can register some work to anthor object with out referance it.
    /// </summary>
    public interface IMessenger : IObservableObject, IEnableLogger, IDisposable
    {

        #region Is registered

        /// <summary>
        /// Whether registered or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsRegistered<T>(string name = null);

        /// <summary>
        /// Whether registered or not.
        /// </summary>
        /// <param name="registor"></param>
        /// <returns></returns>
        bool IsRegistered(object registor);

        #endregion

        #region Register message

        /// <summary>
        /// Register any object message ation.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="action">The action to execute when send this message</param>
        /// <param name="predicate">The predicate filter to set which condition to execute meesage</param>
        /// <param name="scheduler">Set action work this scheduler. </param>
        void Register<T>(Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null);


        /// <summary>
        /// Register <see cref="T"/> ation with the object.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="name">The sign name of the message, if the recipient meesage have the same sign name and satisfy the conditon can execute register action. </param>
        /// <param name="action">The action to execute when send this message</param>
        /// <param name="predicate">The predicate filter to set which condition to execute action.</param>
        /// <param name="scheduler">Set action work this scheduler. </param>
        void Register<T>(string name, Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null) where T : ISigneMessage;


        /// <summary>
        /// Register ation with the type, when get message going to do.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="registorType">this type of message sender, if the recipient meesage have the same sender type and satisfy the conditon can execute register action.</param>
        /// <param name="action">ation do with recipient</param>
        /// <param name="predicate">when the recipient match some condition, can do the action</param>
        /// <param name="scheduler">set the one scheduler</param>
        void Register<T>(Type registorType, Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null) where T : ISenderMessage;

        /// <summary>
        /// Register ation with the object, when get message going to do.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="registor">message sender, if the recipient meesage have the same sender and satisfy the conditon can execute register action.</param>
        /// <param name="action">The action to execute when send this message</param>
        /// <param name="predicate">The predicate filter to set which condition to execute action.</param>
        /// <param name="scheduler">Set action work this scheduler. </param>
        void Register<T>(object registor, Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null) where T : ISenderMessage;


        #endregion



        #region Unregister message

        /// <summary>
        /// Clear the action has registed with the name.
        /// </summary>
        /// <param name="T"></param>
        /// <param name="name">the name of the messae.</param>
        void Unregister<T>(string name = null);


        /// <summary>
        /// clear the action have registed to the object.
        /// </summary>
        /// <param name="registor">the object registed</param>
        void Unregister(object registor);


        #endregion
    }
}
