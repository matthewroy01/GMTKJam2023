using UnityEngine;

namespace MHRUtil.Singletons
{
    public class Singleton<T> : MonoBehaviour where T : Object
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (FindObjectsOfType<T>().Length > 1)
            {
                Destroy(this);
                return;
            }
        
            Instance = GetComponent<T>();
        }
    }
}