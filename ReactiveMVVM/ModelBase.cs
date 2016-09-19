using System;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using ReactiveMVVM.Logging;

namespace ReactiveMVVM
{
    /// <summary>
    /// Model Base.
    /// </summary>
    [DataContract]
    public abstract class ModelBase : INotifyPropertyChanged, IEnableLogger
    {
        /// <summary>
        /// Property changed event handler.
        /// </summary>
        [field: IgnoreDataMember]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        internal void RaisePropertyChanged(string propertyName)
        {
            Contract.Requires(propertyName != null);

            VerifyPropertyName(propertyName);

#if DEBUG
            this.Logger().DebugFormat("{0:X}.{1} changed", this.GetHashCode(), propertyName);
#endif

            var hanler = PropertyChanged;
            if (hanler != null)
            {
                hanler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// Verify property by name.
        /// </summary>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        protected void VerifyPropertyName(string propertyName)
        {
            Contract.Requires(propertyName != null);

            // If you raise PropertyChanged and do not specify a property name,
            // all properties on the object are considered to be changed by the binding system.
            if (String.IsNullOrEmpty(propertyName))
                return;

#if !SILVERLIGHT
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null) {
                string msg = "Invalid property name: " + propertyName;
                this.Logger().Error(msg);
            }
#endif
        }

    }


}
