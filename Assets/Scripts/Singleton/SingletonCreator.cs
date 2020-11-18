using System.Reflection;
using System;
using UnityEngine;

namespace GF
{
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class, ISingleton
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                Debug.LogError("[SingletonCreator]Non-Public Constructor() not found! in " + typeof(T));
            }

            var instance = ctor.Invoke(null) as T;

            instance.OnSingletonInit();

            return instance;
        }
    }
}
