using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHR.GameObjectManagement.Pooling
{
    public class PooledObject<T>
    {
        private T _obj;
        private bool _free = true;
        public T Obj => _obj;

        public PooledObject(T obj)
        {
            _obj = obj;
        }

        public bool GetFree()
        {
            return _free;
        }

        public void SetFree(bool free)
        {
            _free = free;
        }
    }
}