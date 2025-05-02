using System;

namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemo_State
    {
        void HandleEvent(IDemo_Event inputEvent);

        void Entry();

        void Exit();
    }

    public abstract class Base_Demo_State : IDemo_State
    {
        protected IDemo_Context Context { get; }

        public Base_Demo_State(IDemo_Context context) => Context = context;

        public virtual void HandleEvent(IDemo_Event inputEvent) => throw new NotImplementedException($"HandleEvent was not implemented in '{GetType().Name}'.");

        public virtual void Entry() => Console.WriteLine($"Entry '{GetType().Name}' State.");

        public virtual void Exit() => Console.WriteLine($"Exit '{GetType().Name}' State.");
    }

    public class Demo_State_Null : Base_Demo_State
    {
        public Demo_State_Null(IDemo_Context context) : base(context) { }

        public override void HandleEvent(IDemo_Event inputEvent)
        {
            switch (inputEvent)
            {
                case Demo_Event_Create _:
                    Context.SetState(Context.Idle_State);
                    break;

                default:
                    throw new NotImplementedException($"Event '{inputEvent}' was not handled in '{GetType().Name}'.");
            }
        }

        public override void Entry() => Context.Driven.EntryNull();

        public override void Exit() => Context.Driven.ExitNull();
    }

    public class Demo_State_Idle : Base_Demo_State
    {
        public Demo_State_Idle(IDemo_Context context) : base(context) { }

        public override void HandleEvent(IDemo_Event inputEvent)
        {
            switch (inputEvent)
            {
                case Demo_Event_Connect _:
                    Context.Driven.ActionConnectProcess();
                    Context.SetState(Context.Ready_State);
                    break;

                case Demo_Event_Destroy _:
                    Context.SetState(Context.Null_State);
                    break;

                default:
                    throw new NotImplementedException($"Event '{inputEvent}' was not handled in '{GetType().Name}'.");
            }
        }

        public override void Entry() => Context.Driven.EntryIdle();

        public override void Exit() => Context.Driven.ExitIdle();
    }

    public class Demo_State_Ready : Base_Demo_State
    {
        public Demo_State_Ready(IDemo_Context context) : base(context) { }

        public override void HandleEvent(IDemo_Event inputEvent)
        {
            switch (inputEvent)
            {
                case Demo_Event_Disconnect _:
                    Context.Driven.ActionDisconnectProcess();
                    Context.SetState(Context.Idle_State);
                    break;

                default:
                    throw new NotImplementedException($"Event '{inputEvent}' was not handled in '{GetType().Name}'.");
            }
        }

        public override void Entry() => Context.Driven.EntryReady();

        public override void Exit() => Context.Driven.ExitReady();
    }
}