using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHR.GameObjectManagement.Pooling
{
    public class Pool<T>
    {
        protected readonly List<PooledObject<T>> PooledObjects = new List<PooledObject<T>>();
        private const string RAN_OUT_MESSAGE = "MHR.GameObjectManagement.Pool - Ran out of pooled objects to return, returning default of the provided type.";

        public void Initialize(List<T> list)
        {
            foreach (T toPool in list)
            {
                PooledObjects.Add(new PooledObject<T>(toPool));
            }
        }

        public virtual T GetFreeObject()
        {
            foreach(PooledObject<T> pooledObject in PooledObjects)
            {
                if (!pooledObject.GetFree())
                {
                    continue;
                }

                pooledObject.SetFree(false);
                return pooledObject.Obj;
            }

            Debug.LogWarning(RAN_OUT_MESSAGE);
            return default(T);
        }

        public virtual void FreeObject(T obj)
        {
            foreach(PooledObject<T> pooledObject in PooledObjects)
            {
                if (!EqualityComparer<T>.Default.Equals(pooledObject.Obj, obj))
                {
                    continue;
                }

                pooledObject.SetFree(true);
                return;
            }
        }
    }
}