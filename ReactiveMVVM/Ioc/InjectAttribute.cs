using System;

namespace ReactiveMVVM.Ioc
{
    /// <summary>
    /// The attribute to define how to be injected.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {

    }
}
