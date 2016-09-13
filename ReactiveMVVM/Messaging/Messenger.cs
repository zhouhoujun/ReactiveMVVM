using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Collections.Generic;
using ReactiveMVVM.Logging;
using ReactiveMVVM.Subjects;
using System.Diagnostics.Contracts;
using System.Reactive.Disposables;
using ReactiveMVVM;

namespace ReactiveMVVM.Messaging
{
    /// <summary>
    /// The reactive messager.
    /// Creator: Houjun Zhou
    /// </summary>
    public class Messenger : IMessenger
    {
        private readonly Subject<object> _subject = new Subject<object>();
        private static readonly Messenger _default = new Messenger();

        Dictionary<object, List<IDisposable>> disTypeDic = new Dictionary<object, List<IDisposable>>();

        /// <summary>
        /// the default messager.
        /// </summary>
        public static Messenger Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <remarks>
        /// IDisposable object that can be used to unsubscribe the observer from the subject.
        /// </remarks>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<object> observer)
        {
            return _subject
                .DistinctUntilChanged()
                .Subscribe(observer);
        }

        #region Send Message


        public IDisposable Send<T>(T message, int wait = 0)
        {
            return this.SendChannel(_subject, message, wait: wait);
        }

        public IDisposable Send<T>(T message, Action<Exception> onError, int wait = 0)
        {
            return this.SendChannel(_subject, message: message, onError: onError, wait: wait);
        }

        public IDisposable Send<T>(T message, Action onCompleted, int wait = 0)
        {
            return this.SendChannel(_subject, message: message, onCompleted: onCompleted, wait: wait);
        }

        public IDisposable Send<T>(T message, Action<Exception> onError, Action onCompleted, int wait = 0)
        {
            return this.SendChannel(_subject, message: message, onError: onError, onCompleted: onCompleted, wait: wait);
        }

        #endregion


        #region Is registered

        /// <summary>
        /// if is registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsRegistered<T>(string name = null)
        {
            object key;
            if (string.IsNullOrEmpty(name))
                key = typeof(T);
            else
                key = new Tuple<Type, string>(typeof(T), name);
            return disTypeDic.ContainsKey(key);
        }

        /// <summary>
        /// if is registered.
        /// </summary>
        /// <param name="registor"></param>
        /// <returns></returns>
        public bool IsRegistered(object registor)
        {
            return disTypeDic.ContainsKey(registor);
        }

        #endregion

        #region private method


        private IDisposable Subscribe<T>(IObservable<T> source, Action<T> action//, Action<Exception> onError, Action onCompleted
            , Func<T, bool> predicate, IScheduler scheduler)
        {
            IDisposable dis = null;
            //var error = onError == null ? (er => this.Logger().Error(er)) : onError;
            //var completed = onCompleted == null ? (() => { }) : onCompleted;
            var pred = predicate;
            var act = action;
            if (scheduler == null)
            {
                if (pred == null)
                    dis = source.Subscribe(act);//, error, completed);
                else
                    dis = source.Where(pred).Subscribe(act);//, error, completed);
            }
            else
            {
                if (pred == null)
                    dis = source.ObserveOn(scheduler).Subscribe(act);//, error, completed);
                else
                    dis = source.Where(pred).ObserveOn(scheduler).Subscribe(act);//, error, completed);
            }

            return dis;
        }

        private IDisposable Subscribe<T>(Action<T> action //,Action<Exception> onError, Action onCompleted
            , Func<T, bool> predicate, IScheduler scheduler)
        {
            return Subscribe(this.OfType<T>(), action, //onError, onCompleted,
                predicate, scheduler);
        }

        private IDisposable Subscribe<T>(Func<T, bool> localfilter, Action<T> action //, Action<Exception> onError, Action onCompleted
            , Func<T, bool> predicate, IScheduler scheduler)
        {
            return Subscribe(this.OfType<T>().Where(localfilter), action, //onError, onCompleted,
                predicate, scheduler);
        }


        #endregion

        #region Register

        /// <summary>
        /// Register any object message ation.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="action">The action to execute when send this message</param>
        /// <param name="predicate">The predicate filter to set which condition to execute meesage</param>
        /// <param name="scheduler">Set action work this scheduler. </param>
        public void Register<T>(Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null)
        {
            IDisposable dis = Subscribe(action, predicate, scheduler);

            var registorType = typeof(T);
            if (disTypeDic.ContainsKey(registorType))
            {
                var lst = disTypeDic[registorType];
                lst.Add(dis);
                disTypeDic[registorType] = lst;
            }
            else
            {
                disTypeDic.Add(registorType, new List<IDisposable> { dis });
            }
        }


        /// <summary>
        /// Register <see cref="T"/> ation with the object.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="name">The sign name of the message, if the recipient meesage have the same sign name and satisfy the conditon can execute register action. </param>
        /// <param name="action">The action to execute when send this message</param>
        /// <param name="predicate">The predicate filter to set which condition to execute action.</param>
        /// <param name="scheduler">Set action work this scheduler. </param>
        public void Register<T>(string name, Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null) where T : ISigneMessage
        {
            IDisposable dis = Subscribe(a => a.Name == name, action, predicate, scheduler);

            var key = new Tuple<Type, string>(typeof(T), name);
            if (disTypeDic.ContainsKey(key))
            {
                var lst = disTypeDic[key];
                lst.Add(dis);
                disTypeDic[key] = lst;
            }
            else
            {
                disTypeDic.Add(key, new List<IDisposable> { dis });
            }
        }




        /// <summary>
        /// Register ation with the type, when get message going to do.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="registorType">this type of message sender, if the recipient meesage have the same sender type and satisfy the conditon can execute register action.</param>
        /// <param name="action">ation do with recipient</param>
        /// <param name="predicate">when the recipient match some condition, can do the action</param>
        /// <param name="scheduler">set the one scheduler</param>
        public void Register<T>(Type registorType, Action<T> action,  Func<T, bool> predicate = null, IScheduler scheduler = null) where T : ISenderMessage
        {

            IDisposable dis = Subscribe(a => a.Sender.GetType() == registorType, action, predicate, scheduler);

            if (disTypeDic.ContainsKey(registorType))
            {
                var lst = disTypeDic[registorType];
                lst.Add(dis);
                disTypeDic[registorType] = lst;
            }
            else
            {
                disTypeDic.Add(registorType, new List<IDisposable> { dis });
            }
        }

        /// <summary>
        /// Register ation with the object, when get message going to do.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="registor">message sender, if the recipient meesage have the same sender and satisfy the conditon can execute register action.</param>
        /// <param name="action">The action to execute when send this message</param>
        /// <param name="predicate">The predicate filter to set which condition to execute action.</param>
        /// <param name="scheduler">Set action work this scheduler. </param>
        public void Register<T>(object registor, Action<T> action, Func<T, bool> predicate = null, IScheduler scheduler = null) where T : ISenderMessage
        {

            IDisposable dis = Subscribe(a => a.Sender == registor, action, predicate, scheduler);

            if (disTypeDic.ContainsKey(registor))
            {
                var lst = disTypeDic[registor];
                lst.Add(dis);
                disTypeDic[registor] = lst;
            }
            else
            {
                disTypeDic.Add(registor, new List<IDisposable> { dis });
            }
        }

        #endregion

        #region Unregister

        /// <summary>
        /// clear the action have registed.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">The name of the message.</param>
        public void Unregister<T>(string name = null)
        {
            object key;
            if (string.IsNullOrEmpty(name))
                key = typeof(T);
            else
                key = new Tuple<Type, string>(typeof(T), name);
            if (disTypeDic.ContainsKey(key))
            {
                var list = disTypeDic[key];
                disTypeDic.Remove(key);
                foreach (var item in list)
                    item.Dispose();
            }
        }


        /// <summary>
        /// clear the action have registed.
        /// </summary>
        /// <param name="registor"></param>
        public void Unregister(object registor)
        {
            if (disTypeDic.ContainsKey(registor))
            {
                var list = disTypeDic[registor];
                disTypeDic.Remove(registor);
                foreach (var item in list)
                    item.Dispose();
            }
        }


        #endregion

        #region Dispose and clear up.

        ~Messenger()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implement IDisposable, Can unsubscribe all observers and release resources. 
        /// </summary>
        public void Dispose()
        {
            lock (_synchRoot)
            {
                if (_alreadyDisposed)
                {
                    Dispose(true);
                    GC.SuppressFinalize(true);
                }
            }
        }

        object _synchRoot = new object();
        private bool _alreadyDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_alreadyDisposed) return;
            if (disposing)
            {
                if (disTypeDic != null)
                {
                    disTypeDic.Clear();
                    disTypeDic = null;
                }
                if (_subject != null)
                {
                    _subject.Dispose();
                }
                _alreadyDisposed = true;
            }
        }

        #endregion



        
    }
}
