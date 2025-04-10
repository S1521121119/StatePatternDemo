using System;

namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemoState
    {
        void HandleEvent(IDemoEvent stateEvent);
    }

    public abstract class BaseDemoState : IDemoState
    {
        protected IDemoContext Context { get; }

        public BaseDemoState(IDemoContext context) => Context = context;

        public virtual void HandleEvent(IDemoEvent stateEvent)
        {
            throw new NotImplementedException($"事件 {stateEvent} 未實作。");
        }
    }

    public class DemoNullState : BaseDemoState
    {
        public DemoNullState(IDemoContext context) : base(context) { }

        public override void HandleEvent(IDemoEvent stateEvent)
        {
            switch (stateEvent)
            {
                case DemoEvent_Create _:
                    Context.SetState(Context.IdleState);
                    break;

                default:
                    throw new NotImplementedException($"事件 {stateEvent} 在 {GetType().Name} 中未處理。");
            }
        }
    }

    public class DemoIdleState : BaseDemoState
    {
        public DemoIdleState(IDemoContext context) : base(context) { }

        public override void HandleEvent(IDemoEvent stateEvent)
        {
            switch (stateEvent)
            {
                case DemoEvent_Connect _:
                    Context.SetState(Context.ReadyState);
                    Context.Driven.ActionConnectProcess();
                    break;

                case DemoEvent_Destroy _:
                    Context.SetState(Context.NullState);
                    break;

                default:
                    throw new NotImplementedException($"事件 {stateEvent} 在 {GetType().Name} 中未處理。");
            }
        }
    }

    public class DemoReadyState : BaseDemoState
    {
        public DemoReadyState(IDemoContext context) : base(context) { }

        public override void HandleEvent(IDemoEvent stateEvent)
        {
            switch (stateEvent)
            {
                case DemoEvent_Disconnect _:
                    Context.SetState(Context.IdleState);
                    Context.Driven.ActionDisconnectProcess();
                    break;

                default:
                    throw new NotImplementedException($"事件 {stateEvent} 在 {GetType().Name} 中未處理。");
            }
        }
    }
}