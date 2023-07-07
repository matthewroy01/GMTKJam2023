using System.Collections.Generic;

namespace MHR.StateMachine
{
    public class StateMachine
    {
        private State _currentState;
        private List<Connection> _connections = new List<Connection>();
        public State CurrentState => _currentState;

        public StateMachine(State startingState, params Connection[] connections)
        {
            _currentState = startingState;
            _currentState.EnterState();

            _connections = new List<Connection>(connections);
        }

        public bool TryChangeState(State to)
        {
            foreach(Connection connection in _connections)
            {
                if (connection.To != to)
                {
                    continue;
                }

                if (connection.From == _currentState || connection.From == null)
                {
                    _currentState.ExitState();
                    _currentState = to;
                    _currentState.EnterState();
                    return true;
                }
            }

            return false;
        }
    }
}