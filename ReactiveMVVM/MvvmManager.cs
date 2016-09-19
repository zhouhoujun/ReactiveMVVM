using System.Reactive.Concurrency;

namespace ReactiveMVVM
{
    /// <summary>
    /// MVVM manager to setting the 
    /// </summary>
    public static class MvvmManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IScheduler DefaultScheduler;

        static MvvmManager()
        {
#if !PORTABLE
            DefaultScheduler = TaskPoolScheduler.Default;
#else
            DefaultScheduler = DispatcherScheduler.Current;
#endif
        }
    }
}
