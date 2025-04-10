using StatePatternDemo.DemoModule.Interfaces;

namespace StatePatternDemo.DemoModule
{
    public class DemoContext : IDemoContext
    {
        public IDemoState CurrentState { get; private set; }
        public IDemoDriven Driven { get; }
        public IDemoState NullState { get; }
        public IDemoState IdleState { get; }
        public IDemoState ReadyState { get; }

        public DemoContext(IDemoDriven driven)
        {
            Driven = driven;

            NullState = new DemoNullState(this);
            IdleState = new DemoIdleState(this);
            ReadyState = new DemoReadyState(this);

            SetState(NullState); // 初始狀態設為 Null
        }

        public void SetState(IDemoState state)
        {
            CurrentState = state;
        }

        public void HandleEvent(IDemoEvent stateEvent) => CurrentState.HandleEvent(stateEvent);
    }
}