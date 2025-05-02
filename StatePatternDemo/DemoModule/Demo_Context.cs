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
            CurrentState?.Entry();
        }

        public void HandleEvent(IDemo_Event inputEvent) => CurrentState.HandleEvent(inputEvent);
    }
}

// ### State Transition Table
// | No | Current State | Next State | Input Event | Output Action           |
// |----|---------------|------------|-------------|-------------------------|
// | 00 | Null          | Idle       | Create      |                         |
// | 01 | Idle          | Ready      | Connect     | ActionConnectProcess    |
// | 02 | Idle          | Null       | Destroy     |                         |
// | 03 | Ready         | Idle       | Disconnect  | ActionDisconnectProcess |

// ### State Boundary Table
// | No | State | Entry Action | Exit Action |
// |----|-------|--------------|-------------|
// | 00 | Null  | EntryNull    | ExitNull    |
// | 01 | Idle  | EntryIdle    | ExitIdle    |
// | 02 | Ready | EntryReady   | ExitReady   |