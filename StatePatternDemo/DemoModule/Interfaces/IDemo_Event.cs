namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemo_Event { }

    public class Demo_Event_Create : IDemo_Event { }

    public class Demo_Event_Connect : IDemo_Event { }

    public class Demo_Event_Destroy : IDemo_Event { }

    public class Demo_Event_Disconnect : IDemo_Event { }
}