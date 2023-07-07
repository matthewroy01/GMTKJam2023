using UnityEngine;

namespace MHR.Testing
{
    public class TestClass
    {
        public readonly char Character;

        public TestClass()
        {
            Character = (char)Random.Range(65, 90);
        }

        public TestClass(char character)
        {
            Character = character;
        }
    }
}