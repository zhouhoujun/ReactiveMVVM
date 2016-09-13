using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Windows;
using ReactiveMVVM.Logging;
using ReactiveMVVM.Messaging;
using System.Diagnostics;

namespace ReactiveMVVM
{
    /// <summary>
    /// The class for the ViewModel classes, use in the MVVM pattern.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1012",
        Justification = "Constructors should remain public to allow serialization.")]
    [DataContract]
    public abstract class ViewModelBase : ObservableObject, INotifyPropertyChanged, IEnableLogger
    {
        [field: IgnoreDataMember]
        private static bool? _isInDesignMode;
        [field: IgnoreDataMember]
        private IMessenger _messenger;


        /// <summary>
        /// Gets or sets an instance of a <see cref="IMessenger" /> used to
        /// broadcast messages to other objects. If null, this class will
        /// attempt to broadcast using the Messenger's default instance.
        /// </summary>
        [IgnoreDataMember]
        protected IMessenger Messenger
        {
            get
            {
                return _messenger ?? ReactiveMVVM.Messaging.Messenger.Default;
            }
            set
            {
                _messenger = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        [SuppressMessage(
            "Microsoft.Security",
            "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
            Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    //TODO: windows 8
                    //_isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;

                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                        = (bool)DependencyPropertyDescriptor
                                     .FromProperty(prop, typeof(FrameworkElement))
                                     .Metadata.DefaultValue;

                    // Just to be sure
                    if (!_isInDesignMode.Value
                        && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        _isInDesignMode = true;
                    }
#endif
                }

                return _isInDesignMode.Value;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        public ViewModelBase()
            : this(null)
        {
        }



        /// <summary>
        /// constructor.
        /// </summary>
        /// <remarks>
        /// Initializes a new instance of the ViewModelBase class.
        /// </remarks>
        /// <param name="messenger">An instance of a <see cref="Messenger" />
        /// used to broadcast messages to other objects. If null, this class
        /// will attempt to broadcast using the Messenger's default
        /// instance.</param>
        public ViewModelBase(IMessenger messenger)
        {
            _messenger = messenger;
        }


        /// <summary>
        /// To broadcast property changed message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:GenericMethodsShouldProvideTypeParameter",
            Justification = "This syntax is more convenient than other alternatives.")]
        protected override void BroadcastChanged<T>(T oldValue, T newValue, string propertyName)
        {
            base.BroadcastChanged(oldValue, newValue, propertyName);

#if DEBUG
            this.Logger().DebugFormat("Broadcast message, Send message to Messenger 0x{0:X}", Messenger.GetHashCode());
#endif
            //Send meesage to Messager.
            Messenger.Send(new PropertyChangedMessage<T>
            {
                Sender = this,
                OldValue = oldValue,
                NewValue = newValue,
                PropertyName = propertyName
            }
            ,
            error => this.Logger().Error("Broadcast property changed in Messenger error:", error));

        }

    }
}
