using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Threading;
using ReactiveMVVM.Logging;
using ReactiveMVVM.Messaging;
using System.Reactive.Concurrency;
using ReactiveMVVM.Subjects;
using System.Reactive.Linq;
using ReactiveMVVM;

namespace ReactiveMVVM
{

    /// <summary>
    /// The base oberverable object.
    /// </summary>
    [DataContract]
    public abstract class ObservableObject : ModelBase, IObservableObject, INotifyPropertyChanged, IEnableLogger
    {

        [field: IgnoreDataMember]
        private readonly ScheduledSubject<object> _ChangedSubject;

        public ObservableObject(IScheduler scheduer = null)
        {
            scheduer = scheduer ?? MvvmManager.DefaultScheduler;
            _ChangedSubject = new ScheduledSubject<object>(scheduer);
        }

        #region Suppress Change

        [IgnoreDataMember]
        long broadcastSuppressed = 0;

        protected bool CanBroadcast
        {
            get { 
#if SILVERLIGHT
                return broadcastSuppressed == 0; 
#else
                return (Interlocked.Read(ref broadcastSuppressed)==0);
#endif
            }
        }

        /// <summary>
        /// When this method is called, an object will not broadcast message
        /// until the return value is disposed.
        /// </summary>
        /// <returns>
        /// An object that, when disposed, reenables broadcast
        /// </returns>
        public IDisposable SuppressBroadcast()
        {
            Interlocked.Increment(ref broadcastSuppressed);
            return Disposable.Create(() =>
                Interlocked.Decrement(ref broadcastSuppressed));
        }
        #endregion


        /// <summary>
        /// Broadcast
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        protected virtual void BroadcastChanged<T>(T oldValue, T newValue, string propertyName)
        {

#if DEBUG
            this.Logger().DebugFormat("Send message to local ,at subject 0x{0:X}", _ChangedSubject.GetHashCode());
#endif
            //Send message at local.
            this.Send(new PropertyChangedMessage<T>
                {
                    Sender = this,
                    OldValue = oldValue,
                    NewValue = newValue,
                    PropertyName = propertyName
                },
                error=> this.Logger().Error("Broadcast property changed message error:",error)
                );

        }

        #region Send Message

        public IDisposable Send<T>(T message, int wait = 0)
        {
            return this.SendChannel(_ChangedSubject, message, wait: wait);
        }

        public IDisposable Send<T>(T message, Action<Exception> onError, int wait = 0)
        {
            return this.SendChannel(_ChangedSubject, message: message, onError: onError, wait: wait);
        }

        public IDisposable Send<T>(T message, Action onCompleted, int wait = 0)
        {
            return this.SendChannel(_ChangedSubject, message: message, onCompleted: onCompleted, wait: wait);
        }

        public IDisposable Send<T>(T message, Action<Exception> onError, Action onCompleted, int wait = 0)
        {
            return this.SendChannel(_ChangedSubject, message: message, onError: onError, onCompleted: onCompleted, wait: wait);
        }

        #endregion

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="oldValue">The property's value before the change
        /// occurred.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        internal void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue, bool broadcast = false)
        {
            //TODO: 
            if (!CanBroadcast)
            {
                this.Logger().DebugFormat("Change nofification unable.");
                return;
            }
            RaisePropertyChanged(propertyName);
            if (!broadcast) return;
            BroadcastChanged(oldValue, newValue, propertyName);
        }


        

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return _ChangedSubject.Subscribe(observer);
        }


        #region Dispose and clear up.

        ~ObservableObject()
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
                _ChangedSubject.Dispose();
                _alreadyDisposed = true;
            }
        }

        
        

        #endregion

    }
}
