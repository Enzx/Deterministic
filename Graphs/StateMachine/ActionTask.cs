namespace Deterministic.Graphs.StateMachine
{
    public abstract class ActionTask : Task
    {
        private Status _status;
        public Status Execute()
        {
            _status = OnExecute();
            return _status;
        }

        protected virtual Status OnExecute()
        {
            return Status.Success;
        }
    }
}