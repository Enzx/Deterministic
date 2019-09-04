using System.Collections.Generic;
using Deterministic.Graphs.StateMachine;
using NUnit.Framework;

namespace Deterministic.Graphs.Tests
{
    [TestFixture]
    public class FiniteStateMachineTest
    {
        private class UpdateForNthTime : ActionTask
        {
            private int _tries;
            public UpdateForNthTime(int tries)
            {
                _tries = tries;
            }
            protected override Status OnExecute()
            {
                if (_tries > 0)
                {
                    _tries--;
                }
                return _tries == 0 ?
                    Status.Success : Status.Running;
            }
        }

        private class PassAfterNthTime : ConditionTask
        {
            private int _tries = 0;

            public PassAfterNthTime(int tries)
            {
                _tries = tries;
            }

            protected override bool OnCheckCondition()
            {
                if (_tries > 0)
                {
                    _tries--;
                }
                return _tries == 0;
            }
        }

        private class TestState : FSMState
        {
            public List<string> CallbackNames = new List<string>();

            public TestState(FSM fsm) : base(fsm)
            {
            }

            protected override Status OnExecute()
            {
                CallbackNames.Add("OnExecute");
                return base.OnExecute();
            }

            protected override void OnEnter()
            {
                CallbackNames.Add("OnEnter");

                base.OnEnter();
            }

            protected override void OnUpdate()
            {
                if (CallbackNames.Contains("OnUpdate") == false)
                    CallbackNames.Add("OnUpdate");

                base.OnUpdate();
            }

            protected override void OnExit()
            {
                CallbackNames.Add("OnExit");

                base.OnExit();
            }
        }


        [Test]
        public void AddState()
        {
            FSM fsm = new FSM();
            FSMState fsmState = new FSMState(fsm);
            fsm.AddState(fsmState);

        }

        [Test]
        public void FailToStartWithNullState()
        {
            FSM fsm = new FSM();
            FSMState state = new FSMState(fsm);
            fsm.SetFirstNode(state);
            fsm.Start();
        }

        [Test]
        public void TransitionToAnotherState()
        {
            FSM fsm = new FSM();
            FSMState state1 = new FSMState(fsm);
            FSMState state2 = new FSMState(fsm);
            FSMState state3 = new FSMState(fsm);
            FSMConnection connection = new FSMConnection(state2, state3, fsm);
            connection.SetCondition(new PassAfterNthTime(3));
            state1.ConectTo(state2);
            fsm.SetFirstNode(state1);
            fsm.Start();
            for (int i = 0; i < 4; i++)
            {
                fsm.Update();
            }
            Assert.AreEqual(state3, fsm.CurrentState);
        }

        [Test]
        public void TestStateCallbacks()
        {

            List<string> CallbackNames = new List<string>
            {"OnExecute", "OnEnter", "OnUpdate", "OnExit"};

            FSM fsm = new FSM();
            TestState state1 = new TestState(fsm);
            state1.SetJob(new UpdateForNthTime(2));
            TestState state2 = new TestState(fsm);
            var connection = state1.ConectTo(state2, new PassAfterNthTime(4));

            fsm.SetFirstNode(state1);
            fsm.Start();
            for (int i = 0; i < 5; i++)
            {
                fsm.Update();
            }

            Assert.AreEqual(CallbackNames[0], state1.CallbackNames[0]);
            Assert.AreEqual(CallbackNames[1], state1.CallbackNames[1]);
            Assert.AreEqual(CallbackNames[2], state1.CallbackNames[2]);
            Assert.AreEqual(CallbackNames[3], state1.CallbackNames[3]);
        }
    }

}
