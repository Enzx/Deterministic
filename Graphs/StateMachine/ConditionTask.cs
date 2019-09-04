namespace Deterministic.Graphs.StateMachine
{
    public abstract class ConditionTask : Task
    {
        public bool CheckCondition()
        {
            return OnCheckCondition();
        }

        protected abstract bool OnCheckCondition();
    }
}