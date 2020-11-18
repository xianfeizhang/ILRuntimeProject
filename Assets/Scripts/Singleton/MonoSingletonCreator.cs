using UnityEngine;

namespace GF
{
    public static class MonoSingletonCreator
    {
        public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton
        {
            T instance = null;

            if (!Application.isPlaying)
            {
                return instance;
            }

            instance = Object.FindObjectsOfType<T>() as T;

            if (instance != null)
            {
                instance.OnSingletonInit();
                return instance;
            }

            if (instance == null)
            {
                var obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }

            instance.OnSingletonInit();
            return instance;
        }
    }
}
