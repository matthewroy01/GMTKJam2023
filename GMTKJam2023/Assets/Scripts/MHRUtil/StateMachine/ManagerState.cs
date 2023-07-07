namespace MHR.StateMachine
{
    public abstract class ManagerState<T> : State
    {
        private T _manager;
        protected T Manager => _manager;

        public void SetManager(T manager)
        {
            _manager = manager;
        }
        
        public abstract override void EnterState();
        public abstract override void ProcessState();
        public abstract override void ProcessStateFixed();
        public abstract override void ExitState();
    }
}