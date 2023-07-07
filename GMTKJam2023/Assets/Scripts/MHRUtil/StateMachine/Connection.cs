namespace MHR.StateMachine
{
    public class Connection
    {
        private State _from;
        private State _to;
        public State From => _from;
        public State To => _to;

        public Connection(State from, State to)
        {
            _from = from;
            _to = to;
        }

        public Connection(State to)
        {
            _from = null;
            _to = to;
        }
    }
}