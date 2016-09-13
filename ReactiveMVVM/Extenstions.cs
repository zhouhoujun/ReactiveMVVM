
using System;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;
using System.Windows.Threading;
using System.Reactive.Linq;
using ReactiveMVVM.Logging;
using System.Linq.Expressions;
using ReactiveMVVM.Messaging;
using System.Reactive.Disposables;
using System.Threading;

namespace ReactiveMVVM
{

    public static class ModelExtensions
    {
        /// <summary>
        /// set it new value, raise event PropertyChangedEventHandler and broadcast property changed message if changed.
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">the object that owner of the changed property. </param>
        /// <param name="propertyExpression">property expression</param>
        /// <param name="oldvalue">old value of the property.</param>
        /// <param name="newValue">new value of the property.</param>
        /// <returns></returns>
        public static T RaiseAndSetIfChanged<Tobj, T>(this Tobj obj, Expression<Func<Tobj, T>> propertyExpression, ref T oldvalue, T newValue) where Tobj : ModelBase
        {
            if (!object.Equals(oldvalue, newValue))
            {
                var value = oldvalue;
                oldvalue = newValue;
                var body = propertyExpression.Body as MemberExpression;
                obj.RaisePropertyChanged(body.Member.Name);
            }

            return oldvalue;
        }
    }

    public static class ObservableExtenstions
    {
        /// <summary>
        /// set it new value, raise event PropertyChangedEventHandler and broadcast property changed message if changed.
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">the object that owner of the changed property. </param>
        /// <param name="propertyExpression">property expression</param>
        /// <param name="oldvalue">old value of the property.</param>
        /// <param name="newValue">new value of the property.</param>
        /// <param name="broadcast">if broadcast the property changed message.</param>
        /// <returns></returns>
        public static T RaiseAndSetIfChanged<Tobj, T>(this Tobj obj, Expression<Func<Tobj, T>> propertyExpression, ref T oldvalue, T newValue, bool broadcast = false) where Tobj : ObservableObject
        {
            if (!object.Equals(oldvalue, newValue))
            {
                var value = oldvalue;
                oldvalue = newValue;
                var body = propertyExpression.Body as MemberExpression;
                obj.RaisePropertyChanged(body.Member.Name, value, newValue, broadcast);
            }

            return oldvalue;
        }

        /// <summary>
        /// Observe property, Can subscribe action when it change.
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static IObservable<IPropertyChangedMessage<T>> ObservableProperty<Tobj, T>(this Tobj obj, Expression<Func<Tobj, T>> propertyExpression) where Tobj : ObservableObject
        {
            var body = propertyExpression.Body as MemberExpression;
            return obj.OfType<IPropertyChangedMessage<T>>()
                      .Where(x => x.PropertyName == body.Member.Name);
        }


        /// <summary>
        /// Set Observable observe on the MVVM default secheduler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<T> ObserveOnDefaultScheduler<T>(this IObservable<T> source)
        {
            return source.ObserveOn(MvvmManager.DefaultScheduler);
        }

        /// <summary>
        /// convert as type.
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IObservable<TRes> OfType<Tobj, TRes>(this IObservable<Tobj> source, Func<Tobj, TRes> converter)
        {
            return from item in source
                   where item is TRes
                   select converter(item);

        }


        /// <summary>
        /// send the message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="obj">the ObservableObject instance.</param>
        /// <param name="subject">the subject to observe on this message.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="onError">To do deal with errors when send message.</param>
        /// <param name="onCompleted">When send message completed to do the onCompleted action.</param> 
        /// <param name="wait">wait some milliseconds to send the message. </param>
        /// <returns>return the disposable, if dispose it means to cancel the send.</returns>
        public static IDisposable SendChannel<T>(this IObservableObject obj, ISubject<object> subject, T message, Action<Exception> onError = null, Action onCompleted = null, int wait = 0)
        {
            //Contract.Ensures(message != null, "message is null");
            if (message == null)
                return Disposable.Empty;

            try
            {
                var completed = onCompleted == null ? () => { } : onCompleted;
                var error = onError == null ? ex => obj.Logger().Error(ex) : onError;

                var channel = Observable.Return<T>(message);
                if (wait > 0)
                    channel = channel.Throttle(TimeSpan.FromMilliseconds(wait));



                return channel.Subscribe(x => subject.OnNext(x), error, completed);

            }
            catch (Exception e)
            {
                obj.Logger().Error("Message send error: ", e);

                subject.OnError(e);

                return Disposable.Empty;
            }
        }



        static readonly LoggerForObservable ObservableLogger = new LoggerForObservable();
        /// <summary>
        /// Use to log message. Help you debug programe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="This"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IObservable<T> DebugObservable<T>(this IObservable<T> This, string message = "")
        {
            int hash = This.GetHashCode();
            return This.Do(
                x => ObservableLogger.Logger().InfoFormat("0x{0:X} '{1}' OnNext: {2}", hash, message, x),
                ex => ObservableLogger.Logger().Info(String.Format("0x{0:X} '{1}' OnError", hash, message), ex),
                () => ObservableLogger.Logger().InfoFormat("0x{0:X} '{1}' OnCompleted", hash, message));
        }
    }

}



