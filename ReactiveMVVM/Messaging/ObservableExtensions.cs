using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Collections;
using System.Linq;

namespace ReactiveMVVM.Messaging
{

    //internal static class ObservableExtensions
    //{
    //    private static object key = new object();
    //    public static IObservable<T> MessageQueue<T>(this IObservable<T> source, MessageQueue<T> queue, Action onCompleted = null)
    //    {
    //        return Observable.Create<T>(obser =>
    //            {
    //                return source
    //                    .Where(a => a != null)
    //                    .Subscribe(
    //                        x =>
    //                        {
    //                            queue.Enqueue(x);

    //                        },
    //                        error => obser.OnError(error),
    //                        onCompleted == null ? (() => { }) : onCompleted
    //                    );
    //            });

    //    }
    //}


    //public class MessageQueue<T> : IEnumerable<T>, ICollection
    //{
    //    private List<T> _list = new List<T>();
    //    private readonly object key = new object();
    //    /// <summary>
    //    /// Removes and returns the object at the beginning of the <see cref="MessageQueue<T>"/>
    //    /// </summary>
    //    /// <returns>The object that is removed from the beginning of the queue. If empty return null.</returns>
    //    public T Dequeue()
    //    {
    //        T item = default(T);
    //        if (_list.Count > 0)
    //        {
    //            lock (key)
    //            {
    //                item = _list[0];
    //                _list.Remove(item);

    //            }
    //        }
    //        return item;
    //    }


    //    /// <summary>
    //    /// Adds an object to the end of the <see cref="MessageQueue<T>"/>
    //    /// </summary>
    //    /// <param name="item">The object to add to the <see cref="MessageQueue<T>"/>. If is null will not do nothing</param>
    //    public void Enqueue(T item)
    //    {
    //        Attribute.GetCustomAttribute(item.GetType(), typeof(MessageAttribute));

    //        //TODO:
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return _list.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }

    //    public void CopyTo(Array array, int index)
    //    {
    //        lock (key)
    //        {
    //            _list.CopyTo(array.Cast<T>().ToArray(), index);
    //        }
    //    }

    //    public int Count
    //    {
    //        get { return _list.Count; }
    //    }

    //    public bool IsSynchronized
    //    {
    //        get { return true; }
    //    }

    //    public object SyncRoot
    //    {
    //        get { return key; }
    //    }
    //}


}
