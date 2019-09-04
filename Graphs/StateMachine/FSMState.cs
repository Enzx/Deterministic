namespace Deterministic.Graphs.StateMachine
{
    public class FSMState : Node
    {
        private ActionTask _task;
        private readonly FSM _fsm;

        public FSMState(FSM fsm) : base(fsm)
        {
            _fsm = fsm;
        }

        public void SetJob(ActionTask task)
        {
            _task = task;
        }

        protected override Status OnExecute()
        {
            Status = Status.Running;

            Enter();
            return Status;
        }

        private void CheckConditions()
        {
            for (int i = 0; i < OutConnections.Count; i++)
            {
                FSMConnection connection = (FSMConnection)OutConnections[i];
                if (connection.CheckCondition())
                {
                    connection.ApplyTransition();
                    Exit();
                }
            }
        }

        protected void Enter()
        {
            OnEnter();
        }

        protected virtual void OnEnter() { }

        public virtual void Update()
        {
            if (_task == null ||
                _task.Execute() == Status.Success)
            {
                Finish();
            }

            if (Status == Status.Running)
            {
                OnUpdate();
            }
            else
            {
                CheckConditions();
            }
        }

        protected virtual void OnUpdate() { }

        protected void Exit()
        {
            OnExit();
        }

        protected virtual void OnExit() { }

        protected void Finish()
        {
            Status = Status.Success;

        }

        public FSMConnection ConectTo(FSMState anotherState)
        {
            return new FSMConnection(this, anotherState, _fsm);
        }

        public FSMConnection ConectTo(FSMState anotherState, ConditionTask condition)
        {
            FSMConnection connection = new FSMConnection(this, anotherState, _fsm);
            connection.SetCondition(condition);
            return connection;

        }
    }
}