using UnityEngine;

namespace GFScripts
{
    public class Singleton<T> where T : new()
    {
        private static T instance_;

        public static T instance
        {
            get
            {
                if (instance_ == null)
                {
                    Debug.LogFormat("Create singleton instance for class {0}", typeof(T).Name);
                    instance_ = new T();
                }
                return instance_;
            }
        }

        public static bool HasInstance()
        {
            return instance_ != null;
        }

        public static void ClearInstance()
        {
            instance_ = default(T);
        }

        public Singleton()
        {
            if (instance_ != null)
            {
                Debug.LogError("单例对象只能创建一个实力对象: " + this);
            }
        }
    }
}
