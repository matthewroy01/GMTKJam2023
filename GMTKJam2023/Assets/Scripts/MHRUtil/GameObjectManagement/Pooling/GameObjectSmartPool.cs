using System;
using UnityEngine;
using System.Collections.Generic;

namespace MHR.GameObjectManagement.Pooling
{
    public class GameObjectSmartPool<T> : SmartPool<T>
    {
        private GameObject _original;
        private Transform _parent;

        public void Initializel(List<GameObject> list, GameObject original, Transform parent = null)
        {
            foreach (GameObject toPool in list)
            {
                PooledObjects.Add(new PooledObject<T>((T)Convert.ChangeType(toPool, typeof(T))));
            }

            _original = original;
            _parent = parent;
        }

        public override T GetFreeObject()
        {
            return base.GetFreeObject();
        }

        protected override T CreateNew()
        {
            GameObject tmp = UnityEngine.Object.Instantiate(_original, _parent);
            return tmp.GetComponent<T>();
        }

        public override void FreeObject(T obj)
        {
            base.FreeObject(obj);
        }
    }
}