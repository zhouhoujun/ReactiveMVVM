using System;


#if  WINDOWS_PHONE
/// <summary>
/// Create by self. To same with .Net framework.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
internal class Tuple<T1, T2>
{
    public Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
        var hash1 = (item1 != null) ? item1.GetHashCode() : 0;
        var hash2 = (item2 != null) ? item2.GetHashCode() : 0;
        hash = hash1 ^ hash2;
    }
    public Tuple() { }

    private int hash;
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }



    public override bool Equals(object obj)
    {
        var other = obj as Tuple<T1, T2>;
        if (other == null)
            return false;

        bool equals1 = (Item1 != null) ? Item1.Equals(other.Item1) : other.Item1 == null;
        bool equals2 = (Item2 != null) ? Item2.Equals(other.Item2) : other.Item2 == null;
        return equals1 && equals2;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}
#endif



#if WINDOWS_PHONE

namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Create by self. To same with .Net framework.
    /// </summary>
    internal class ContractInvariantMethodAttribute : Attribute { }
    ///// <summary>
    ///// Create by self. To same with .Net framework.
    ///// </summary>
    //internal class Contract
    //{
    //    public static void Requires(bool b, string s = null) { }
    //    public static void Ensures(bool b, string s = null) { }
    //    public static void Invariant(bool b, string s = null) { }
    //    public static T Result<T>() { return default(T); }
    //}
}

#endif

#if WINDOWS_PHONE
namespace System.Concurrency
{
    /// <summary>
    /// Lazy class. Create by self. To same with .Net framework.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Lazy<T>
    {
        public Lazy(Func<T> ValueFetcher)
        {
            _Value = ValueFetcher();
        }

        T _Value;
        T Value
        {
            get { return _Value; }
        }
    }

}
#endif
