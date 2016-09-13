using System;
using System.Windows.Input;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;
using ReactiveMVVM.Logging;
using System.Diagnostics;
using System.Reactive.Linq;
using ReactiveMVVM.Subjects;

namespace ReactiveMVVM.Commands
{
    /// <summary>
    /// The reactive command.
    /// </summary>
    public class RxCommand : IRxCommand
    {
        ScheduledSubject<object> _ExecuteSubject;
        Predicate<object> _CanExecute;
        ScheduledSubject<bool> _CanExecuteSubject;

        /// <summary>
        /// Fires whenever the CanExecute of the ICommand changes. 
        /// </summary>
        public IObservable<bool> CanExecuteObservable
        {
            get { return _CanExecuteSubject.DistinctUntilChanged(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// constructor. 
        /// Create a new command, You can subscribe excute action by this instance.
        /// </summary>
        /// <param name="canExecute">Judage if can excute the command</param>
        /// <param name="scheduer"></param>
        public RxCommand(Predicate<object> canExecute = null, IScheduler scheduer = null)
        {
            this._CanExecute = canExecute;
            InitilizeSubject(scheduer);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dowork">The action command to do</param>
        /// <param name="canExecute">Judage if can excute this command</param>
        /// <param name="onError">To deal with excute error</param>
        /// <param name="afterCompleteWork">After command excute completed to do this action</param>
        /// <param name="scheduer">Represents an object that schedules units of work</param>
        public RxCommand(Action<object> dowork, Predicate<object> canExecute = null, Action<Exception> onError = null, Action afterCompleteWork = null, IScheduler scheduer = null)
        {
            this._CanExecute = canExecute;
            InitilizeSubject(scheduer);

            if (dowork != null)
            {
                _ExecuteSubject.Subscribe(
                    x => dowork(x),
                    err =>
                    {
                        this.Logger().Error(err.ToString());
                        if (onError != null)
                            onError(err);
                    },
                    afterCompleteWork ?? (() => { })
                 );
            }

        }

        void InitilizeSubject(IScheduler scheduer)
        {
            scheduer = scheduer ?? MvvmManager.DefaultScheduler;
            _CanExecuteSubject = new ScheduledSubject<bool>(scheduer);

            //TODO: here
            //var handler = CanExecuteChanged;
            _CanExecuteSubject.DistinctUntilChanged()
                .Subscribe(x =>
            {
                var handler = CanExecuteChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            },
            ex => this.Logger().Error(ex));

            _ExecuteSubject = new ScheduledSubject<object>(scheduer);
        }

        /// <summary>
        /// To judge if the command can excute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (_CanExecute != null)
            {
                var can = _CanExecute(parameter);
                _CanExecuteSubject.OnNext(can);
                return can;
            }
            else
            {
                _CanExecuteSubject.OnNext(true);
                return true;
            }
        }

        /// <summary>
        /// To be called when the command is invoked
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                this.Logger().InfoFormat("{0:X}: Excuted", this.GetHashCode());
                _ExecuteSubject.OnNext(parameter);
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
        public IDisposable Subscribe(IObserver<object> observer)
        {
            return _ExecuteSubject.Subscribe(observer);
        }

        
        #region Dispose and clear up.

        ~RxCommand()
        {
            Dispose(false);
        }

        object _synchRoot = new object();
        private bool _alreadyDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_alreadyDisposed) return;
            if (disposing)
            {
                if (_ExecuteSubject != null)
                {
                    _ExecuteSubject.Dispose();
                    _ExecuteSubject = null;
                }
                if (_CanExecuteSubject != null)
                {
                    _CanExecuteSubject.Dispose();
                    _CanExecuteSubject = null;
                }
                _alreadyDisposed = true;
            }
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

        #endregion
    }

    /// <summary>
    /// The reactive command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RxCommand<T> : IRxCommand<T>
    {
        ScheduledSubject<T> _ExecuteSubject;
        Predicate<T> _CanExecute;
        ScheduledSubject<bool> _CanExecuteSubject;

        /// <summary>
        /// Fires whenever the CanExecute of the ICommand changes. 
        /// </summary>
        public IObservable<bool> CanExecuteObservable
        {
            get { return _CanExecuteSubject.DistinctUntilChanged(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="scheduer"></param>
        public RxCommand(Predicate<T> canExecute = null, IScheduler scheduer = null)
        {
            this._CanExecute = canExecute;
            InitilizeSubject(scheduer);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dowork">The action command to do</param>
        /// <param name="canExecute">Judage if can excute</param>
        /// <param name="onError">To deal with excute error</param>
        /// <param name="afterCompleteWork">After command excute completed to do this action</param>
        /// <param name="scheduer">Represents an object that schedules units of work</param>
        public RxCommand(Action<T> dowork, Predicate<T> canExecute = null, Action<Exception> onError=null, Action afterCompleteWork=null, IScheduler scheduer = null)
        {
            this._CanExecute = canExecute;
            InitilizeSubject(scheduer);

            if (dowork != null)
            {
                _ExecuteSubject.Subscribe(
                    x => dowork(x),
                    err =>
                    {
                        this.Logger().Error(err.ToString());
                        if (onError != null)
                            onError(err);
                    },
                    afterCompleteWork ?? (() => { })
                 );
            }
                
        }

        void InitilizeSubject(IScheduler scheduer)
        {
            scheduer = scheduer ?? MvvmManager.DefaultScheduler;
            _CanExecuteSubject = new ScheduledSubject<bool>(scheduer);

            //TODO: here
            //var handler = CanExecuteChanged;
            _CanExecuteSubject.DistinctUntilChanged()
                .Subscribe(x =>
            {
                var handler = CanExecuteChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            },
            ex => this.Logger().Error(ex));

            _ExecuteSubject = new ScheduledSubject<T>(scheduer);
        }


        /// <summary>
        /// To judge if the command can excute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (_CanExecute != null)
            {
                var can = _CanExecute((T)parameter);
                _CanExecuteSubject.OnNext(can);
                return can;
            }
            else
            {
                _CanExecuteSubject.OnNext(true);
                return true;
            }
        }

        /// <summary>
        /// To be called when the command is invoked
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                this.Logger().InfoFormat("{0:X}: Excuted", this.GetHashCode());
                _ExecuteSubject.OnNext((T)parameter);
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
            return _ExecuteSubject.Subscribe(observer);
        }

        #region Dispose and clear up.

        ~RxCommand()
        {
            Dispose(false);
        }

        private bool _alreadyDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_alreadyDisposed) return;
            if (disposing)
            {
                if (_ExecuteSubject != null)
                {
                    _ExecuteSubject.Dispose();
                    _ExecuteSubject = null;
                }
                if (_CanExecuteSubject != null)
                {
                    _CanExecuteSubject.Dispose();
                    _CanExecuteSubject = null;
                }
                _alreadyDisposed = true;
            }
        }

        object _synchRoot = new object();
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

        #endregion

    }
}
