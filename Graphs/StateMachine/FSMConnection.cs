namespace Deterministic.Graphs.StateMachine
{
    public class FSMConnection : Connection
    {
        private ConditionTask _condition;
        private FSM _fsm;
        public FSMConnection(Node fromNode, Node toNode, FSM fsm)
            : base(fromNode, toNode, fsm)
        {
            _fsm = fsm;
        }

        public bool CheckCondition()
        {
            return _condition == null || _condition.CheckCondition();
        }

        public void SetCondition(ConditionTask condition)
        {
            _condition = condition;
        }

        public void ApplyTransition()
        {

            _fsm.ChangeState((FSMState)ToNode);
        }
    }
}