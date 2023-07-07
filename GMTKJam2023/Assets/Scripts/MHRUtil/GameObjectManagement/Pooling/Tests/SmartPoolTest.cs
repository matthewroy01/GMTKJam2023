using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MHR.GameObjectManagement.Pooling;

namespace MHR.Testing
{
    public class SmartPoolTest : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _displayText;
        private SmartPool<TestClass> _pool = new SmartPool<TestClass>();
        private List<TestClass> _activeObjects = new List<TestClass>();

        private void Awake()
        {
            List<TestClass> list = new List<TestClass>();
            char character = 'A';

            for(int i = 0; i < 6; ++i)
            {
                TestClass tmp = new TestClass(character);
                list.Add(tmp);
                character++;
            }

            _pool.Initialize(list);
        }

        public void EnableInPool()
        {
            TestClass tmp = _pool.GetFreeObject();

            if (tmp == null)
            {
                return;
            }

            _activeObjects.Add(tmp);
            UpdateText();
        }

        public void DisableInPool()
        {
            if (_activeObjects.Count == 0)
            {
                return;
            }

            int index = Random.Range(0, _activeObjects.Count);
            _pool.FreeObject(_activeObjects[index]);
            _activeObjects.RemoveAt(index);
            UpdateText();
        }

        public void UpdateText()
        {
            string text = "";

            foreach(TestClass activeObject in _activeObjects)
            {
                text += activeObject.Character + "\n";
            }

            _displayText.text = text;
        }
    }
}