namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemoContext
    {
        IDemoState CurrentState { get; }
        IDemoState NullState { get; }
        IDemoState IdleState { get; }
        IDemoState ReadyState { get; }

        IDemoDriven Driven { get; }

        void SetState(IDemoState state);

        void HandleEvent(IDemoEvent stateEvent);
    }
}