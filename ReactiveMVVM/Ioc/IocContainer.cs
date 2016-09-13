using System;
using System.Linq;
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
using ReactiveMVVM.Logging;
using System.Collections;

namespace ReactiveMVVM.Ioc
{
    ////Future, Not work now!
    public class IocContainer : IContainer, IEnableLogger
    {

        //Register will cast as factory.
        private readonly Dictionary<Tuple<Type, string>, Func<Object>> _factoryDict = new Dictionary<Tuple<Type, string>, Func<Object>>();

        private readonly object _lock = new object();

        private Tuple<Type, string> GetKey(Type type, string key)
        {
            return new Tuple<Type, string>(type, key == null ? null : key.Trim());
        }

        protected virtual object CreateByType(Type type)
        {
            var constructor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();
            if (constructor == null)
                throw new ResolveException("Could not locate a constructor for type: " + type.FullName);

            var constructorParams = new List<object>(constructor.GetParameters().Length);
            foreach (var parameterInfo in constructor.GetParameters())
            {
                object parameter = null;
                try
                {
                    string key = string.Format("{0}.{1}", type.Name, parameterInfo.Name);
                    parameter = GetInstance(null, key);
                }
                catch (Exception)
                {
                    parameter = GetInstance(parameterInfo.ParameterType);
                }
                finally
                {
                    constructorParams.Add(parameter);
                }
            }

            try
            {
                return constructor.Invoke(constructorParams.ToArray());
            }
            catch (Exception exception)
            {
                this.Logger().Error("Failed to resolve " + type.Name, exception);
                throw new ResolveException("Failed to resolve " + type.Name, exception);
            }
        }

        public bool IsRegistered<T>(string key = null)
        {
            var typekey = GetKey(typeof(T), key);

            if (string.IsNullOrEmpty(key))
            {
                if (_factoryDict.ContainsKey(typekey))
                    return true;
                return _factoryDict.ContainsKey(typekey);
            }
            else
            {
                return false;
            }
        }


        public IContainer Register<T>(T instance, string key = null)
        {
            lock (_lock)
            {
                var typekey = GetKey(typeof(T), key);
                _factoryDict[typekey] = () => instance;
            }
            return this;
        }


        public IContainer Register<T>(string key = null) where T : class
        {
            lock (_lock)
            {
                var typekey = GetKey(typeof(T), key);

                _factoryDict[typekey] = () => CreateByType(typeof(T));
            }

            return this;
        }



        public IContainer Register<TBase, TClass>(string key = null) where TClass : class, TBase
        {
            lock (_lock)
            {
                var typekey = GetKey(typeof(TBase), key);

                _factoryDict[typekey] = () => CreateByType(typeof(TClass));
            }

            return this;
        }

        public IContainer Register<T>(Func<T> factory, string key = null)
        {
            lock (_lock)
            {
                var typekey = GetKey(typeof(T), key);

                _factoryDict[typekey] = () => factory();
            }

            return this;
        }



        public IEnumerable<T> GetAllInstances<T>()
        {
            return _factoryDict.Keys.Where(a => a.Item1 == typeof(T)).Select(o => _factoryDict[o]()).OfType<T>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _factoryDict.Keys.Where(a => a.Item1 == serviceType).Select(o => _factoryDict[o]());
        }

        private object GetInstnace(string key)
        {
            var typekey = _factoryDict.Keys.FirstOrDefault(a => a.Item2 == key);
            return typekey == null ? null : _factoryDict[typekey]();
        }

        public T GetInstance<T>()
        {
            var obj = GetInstance(typeof(T), null);
            return obj == null ? default(T) : (T)obj;
        }

        public T GetInstance<T>(string key)
        {
            var obj = GetInstance(typeof(T), key);
            return obj == null ? default(T) : (T)obj;
        }

        public object GetInstance(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        public object GetInstance(Type serviceType, string key)
        {
            if (serviceType == null)
            {

            }
            var typekey = GetKey(serviceType, key);
            if (_factoryDict.ContainsKey(typekey))
                return _factoryDict[typekey]();

            return null;
        }

        public IContainer Unregister<T>(string key = null) where T : class
        {
            return Unregister(typeof(T), key);
        }

        public IContainer Unregister<T>(T instance) where T : class
        {
            return Unregister(typeof(T), null);
        }

        private IContainer Unregister(Type type, string key)
        {
            var typekey = GetKey(type, key);
            if (_factoryDict.ContainsKey(typekey))
                _factoryDict.Remove(typekey);
            return this;
        }



    }
}
