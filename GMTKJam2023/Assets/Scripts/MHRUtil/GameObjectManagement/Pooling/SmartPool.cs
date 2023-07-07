using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MHR.GameObjectManagement.Pooling
{
    public class SmartPool<T> : Pool<T>
    {
        private T _original;

        public void Initialize(List<T> list, T original)
        {
            Initialize(list);

            _original = original;
        }

        public override T GetFreeObject()
        {
            foreach (PooledObject<T> pooledObject in PooledObjects)
            {
                if (!pooledObject.GetFree())
                {
                    continue;
                }

                pooledObject.SetFree(false);
                return pooledObject.Obj;
            }

            return CreateNew();
        }

        protected virtual T CreateNew()
        {
            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);
            return (T)obj;
        }

        public override void FreeObject(T obj)
        {
            base.FreeObject(obj);
        }
    }
}