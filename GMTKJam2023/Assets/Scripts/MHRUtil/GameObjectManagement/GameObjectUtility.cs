using UnityEngine;

namespace MHR.GameObjectManagement
{
    public static class GameObjectUtility
    {
        public static void DontDestroyOnLoad<T>(Object obj)
        {
            Object[] objs = Object.FindObjectsOfType(typeof(T));

            if (objs.Length > 1)
            {
                Object.Destroy(obj);
                return;
            }

            Object.DontDestroyOnLoad(obj);
        }
    }
}