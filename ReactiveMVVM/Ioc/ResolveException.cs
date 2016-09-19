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

namespace ReactiveMVVM.Ioc
{
    /// <summary>
    /// Resolve object instance error exception.
    /// </summary>
    public class ResolveException : Exception
    {
        public ResolveException()
        {
        }

        public ResolveException(string message)
            : base(message)
        {
        }

        public ResolveException(string message, Exception e)
            : base(message, e)
        {
        }
    }
}
