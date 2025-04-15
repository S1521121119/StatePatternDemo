namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemo_Context
    {
        IDemo_State CurrentState { get; }
        IDemo_State Null_State { get; }
        IDemo_State Idle_State { get; }
        IDemo_State Ready_State { get; }

        IDemo_Driven Driven { get; }

        void SetState(IDemo_State state);

        void HandleEvent(IDemo_Event stateEvent);
    }
}