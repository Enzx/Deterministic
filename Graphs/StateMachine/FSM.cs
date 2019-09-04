namespace Deterministic.Graphs.StateMachine
{
    public class FSM : Graph
    {
        private FSMState _currentState;

        public FSMState CurrentState => _currentState;

        public FSM()
        {
        }

        public void AddState(FSMState fsmState)
        {
            Nodes.Add(fsmState);
        }

        public void Start()
        {
            if (CanStart())
            {
                _currentState = (FSMState)FirstNode;
                _currentState.Execute();
            }

            bool CanStart()
            {
                return _currentState == null &&
                       Nodes.Count != 0 &&
                       FirstNode != null;
            }
        }

        public void Update()
        {
            _currentState.Update();
        }


        public void ChangeState(FSMState state)
        {
            _currentState = state;
        }
    }
}
