using UnityEngine;

namespace GF
{
    public class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static object m_Lock = new object();
        private static T m_Instance;
        public static T Instance
        {
            get
            {
                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        Debug.LogFormat("[Singleton]Create singleton instance for class {0}", typeof(T).Name);
                        m_Instance = SingletonCreator.CreateSingleton<T>();
                    }
                    return m_Instance;
                }
            }

        }

        public static bool HasInstance()
        {
            return m_Instance != null;
        }

        public static void Dispose()
        {
            m_Instance = null;
        }

        public void OnSingletonInit()
        {
        }
    }
}
