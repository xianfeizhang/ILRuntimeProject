using UnityEngine;

namespace GF
{
    public class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static T m_Instance = null;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                }
                return m_Instance;
            }
        }

        public void OnSingletonInit()
        {
        }

        private void OnApplicationQuit()
        {
            m_Instance = null;
        }
    }
}
