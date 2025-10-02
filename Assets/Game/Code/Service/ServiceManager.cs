using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Baruah.Service
{
    public static class ServiceManager
    {
        private static Dictionary<System.Type, IService> _services = new();
        
        public static void AddService<T>(T service) where T : class, IService
        {
            _services.Add(typeof(T), service);
            Debug.Log($"Added service: {typeof(T).Name}");
            service.Initialize();
        }

        public static void RemoveService<T>() where T : class, IService
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                service.OnDestroy();
                _services.Remove(typeof(T));
                Debug.Log($"Removed service: {typeof(T).Name}");
            }
        }

        public static T Get<T>() where T : class, IService
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            Debug.LogErrorFormat("Can't find service of type '{0}'", typeof(T).Name);
            return null;
        }

        public static IReadOnlyList<IService> All => _services.Values.ToList(); 

        public static void Update()
        {
            foreach (var service in _services.Values)
            {
                service.Update();
            }
        }
    }
}
