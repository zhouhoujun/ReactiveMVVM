using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM.Commands
{

    /// <summary>
    /// the command with reactive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRxCommand : ICommand, IObservable<object>, IEnableLogger, IDisposable
    {

    }

    /// <summary>
    /// the command with reactive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRxCommand<T> : ICommand, IObservable<T>, IEnableLogger, IDisposable
    {

    }
}
