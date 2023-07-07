using UnityEngine;

namespace MHR.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void ProcessState();
        public abstract void ProcessStateFixed();
    }
}