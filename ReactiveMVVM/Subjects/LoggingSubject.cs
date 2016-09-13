//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.Reactive.Subjects;
//using ReactiveMVVM.Logging;
//using System.Reactive.Concurrency;
//using System.Reactive.Linq;

//namespace ReactiveMVVM.Subjects
//{
//    public class LoggingSubject<T> : ISubject<T>, IDisposable, IEnableLogger
//    {
//        readonly Subject<T> _Subject = new Subject<T>();
//        /// <summary>
//        /// constructor.
//        /// Create a new Subject observe on the one scheduler. 
//        /// </summary>
//        public LoggingSubject()
//        {
//        }


//        /// <summary>
//        /// Unsubscribe all observers and release resources.
//        /// </summary>
//        public void Dispose()
//        {
//            _Subject.Dispose();
//        }

//        /// <summary>
//        /// Notifies all subscribed observers of the end of the sequence.
//        /// </summary>
//        public void OnCompleted()
//        {
//            _Subject.OnCompleted();
//        }

//        /// <summary>
//        /// Notifies all subscribed observers with the exception.
//        /// </summary>
//        /// <param name="error">The exception to send to all subscribed observers.</param>
//        public void OnError(Exception error)
//        {
//            this.Logger().Error("QueueSubject OnNext error: ", error);
//            _Subject.OnError(error);
//        }

//        /// <summary>
//        /// Notifies all subscribed observers with the value.
//        /// </summary>
//        /// <param name="value">The value to send to all subscribed observers.</param>
//        public void OnNext(T value)
//        {
//            try
//            {
//                _Subject.OnNext(value);
//            }
//            catch (Exception e)
//            {
//                OnError(e);
//            }
//        }

//        /// <summary>
//        /// Subscribes an observer to the subject.
//        /// </summary>
//        /// <remarks>
//        /// IDisposable object that can be used to unsubscribe the observer from the subject.
//        /// </remarks>
//        /// <param name="observer">Observer to subscribe to the subject.</param>
//        /// <returns></returns>
//        public IDisposable Subscribe(IObserver<T> observer)
//        {
//            return _Subject //judage when to deal with message. 
//                .DistinctUntilChanged() //remove same one
//                .Subscribe(observer);
//        }
//    }
//}
