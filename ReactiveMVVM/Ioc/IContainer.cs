using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveMVVM.Ioc
{
    /// <summary>
    /// Ioc container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// If this type or with special name is registed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsRegistered<T>(string key = null);


        #region Register object

        /// <summary>
        /// Register one instance
        /// </summary>
        /// <typeparam name="T">They type of base type or interface of the instance.</typeparam>
        /// <param name="instance">instance of the object</param>
        /// <param name="key"></param>
        /// <returns></returns>
        IContainer Register<T>(T instance, string key = null);

        /// <summary>
        /// Registe a type into container.
        /// </summary>
        /// <typeparam name="T">The type of class being registered</typeparam>
        /// <param name="key">registe with a friendly name key to this type.</param>
        /// <returns>return this container self.</returns>
        IContainer Register<T>(string key = null) where T : class;

        /// <summary>
        /// Registe a implement type into container.
        /// </summary>
        /// <typeparam name="TBase">base type or interface</typeparam>
        /// <typeparam name="TClass">Implement the base type or interface</typeparam>
        /// <param name="key">registe with a friendly name key to <see cref="TClass"/>.</param>
        /// <returns>Implement the base type or interface</returns>
        IContainer Register<TBase, TClass>(string key = null) where TClass : class, TBase;

        /// <summary>
        /// Registe a instance factory for T type.
        /// </summary>
        /// <typeparam name="T">The type of class being registered. </typeparam>
        /// <param name="factory">The factory func to create instance of <see cref="T"/>.</param>
        /// <param name="key">registe with a friendly name key to this factory.</param>
        /// <returns></returns>
        IContainer Register<T>(Func<T> factory, string key = null);

        #endregion

        #region Resolve object

        /// <summary>
        /// Get all instances of the given T currently registered in the container.
        /// </summary>
        /// <exception cref="ResolveException">
        /// if there are errors resolving the service instance.
        /// </exception>
        /// <typeparam name="T">Type of object requested.</typeparam>
        /// <returns>A sequence of instances of the requested T.</returns>
        IEnumerable<T> GetAllInstances<T>();

        /// <summary>
        /// Get all instances of the given serviceType currently registered in the container.
        /// </summary>
        /// <exception cref="ResolveException">
        /// if there are errors resolving the service instance.
        /// </exception>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>A sequence of instances of the requested serviceType.</returns>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Get an instance of the given T.
        /// </summary>
        /// <typeparam name="T">Type of object requested.</typeparam>
        /// <exception cref="ResolveException">
        /// if there are errors resolving the service instance.
        /// </exception>
        /// <returns></returns>
        T GetInstance<T>();

        /// <summary>
        /// Get an instance of the given named T.
        /// </summary>
        /// <exception cref="ResolveException">
        /// if there are errors resolving the service instance.
        /// </exception>
        /// <typeparam name="T">Type of object requested.</typeparam>
        /// <param name="key"></param>
        /// <returns>The requested instance.</returns>
        T GetInstance<T>(string key);

        /// <summary>
        /// Get an instance of the given serviceType.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <exception cref="ResolveException">
        /// if there are errors resolving the service instance.
        /// </exception>
        /// <returns>The requested service instance.</returns>
        object GetInstance(Type serviceType);


        /// <summary>
        /// Get an instance of the given named serviceType.
        /// </summary>
        /// <exception cref="ResolveException">
        /// If there are errors resolving the service instance.
        /// </exception>
        /// <param name="serviceType">Type of object requested.</param>
        /// <param name="key"></param>
        /// <returns>The requested service instance.</returns>
        object GetInstance(Type serviceType, string key);


        #endregion

        #region Unregister object

        /// <summary>
        /// unregiste this <see cref="T"/> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        IContainer Unregister<T>(string key = null) where T : class;

        /// <summary>
        /// unregiste this <see cref="T"/> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        IContainer Unregister<T>(T instance) where T : class;

        #endregion

    }
}
