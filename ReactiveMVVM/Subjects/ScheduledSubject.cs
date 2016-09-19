using System;
using System.Reactive.Subjects;
using ReactiveMVVM.Logging;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ReactiveMVVM.Subjects
{
    /// <summary>
    /// This subject observe on the one scheduler. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScheduledSubject<T> : IDisposable, ISubject<T>, IEnableLogger
    {
        /// <summary>
        /// constructor.
        /// Create a new Subject observe on the one scheduler. 
        /// </summary>
        /// <param name="scheduler"></param>
        public ScheduledSubject(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        readonly IScheduler _scheduler;
        readonly Subject<T> _subject = new Subject<T>();

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public void Dispose()
        {
            _subject.Dispose();
        }

        /// <summary>
        /// Notifies all subscribed observers of the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            _subject.OnCompleted();
        }

        /// <summary>
        /// Notifies all subscribed observers with the exception.
        /// </summary>
        /// <param name="error">The exception to send to all subscribed observers.</param>
        public void OnError(Exception error)
        {
            _subject.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed observers with the value.
        /// </summary>
        /// <param name="value">The value to send to all subscribed observers.</param>
        public void OnNext(T value)
        {
            try
            {
                _subject.OnNext(value);
            }
            catch (Exception e)
            {
                this.Logger().Error(e);
                _subject.OnError(e);
            }
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <remarks>
        /// IDisposable object that can be used to unsubscribe the observer from the subject.
        /// </remarks>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_scheduler == null)
                return _subject.ObserveOnDefaultScheduler().Subscribe(observer);
            else
                return _subject.ObserveOn(_scheduler).Subscribe(observer);
        }
    }
}
