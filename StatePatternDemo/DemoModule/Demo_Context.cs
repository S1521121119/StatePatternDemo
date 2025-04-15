using StatePatternDemo.DemoModule.Interfaces;

namespace StatePatternDemo.DemoModule
{
    public class Demo_Context : IDemo_Context
    {
        public IDemo_State CurrentState { get; private set; }
        public IDemo_State Null_State { get; }
        public IDemo_State Idle_State { get; }
        public IDemo_State Ready_State { get; }

        public IDemo_Driven Driven { get; }

        public Demo_Context(IDemo_Driven driven)
        {
            Driven = driven;
            Null_State = new Demo_State_Null(this);
            Idle_State = new Demo_State_Idle(this);
            Ready_State = new Demo_State_Ready(this);

            SetState(Null_State); // The initial state is set to 'Null'
        }

        public void SetState(IDemo_State state)
        {
            if (CurrentState == state) return;
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState?.Enter();
        }

        public void HandleEvent(IDemo_Event stateEvent) => CurrentState.HandleEvent(stateEvent);
    }
}