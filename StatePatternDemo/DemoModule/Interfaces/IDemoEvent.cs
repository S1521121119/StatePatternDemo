namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemoEvent { }

    public class DemoEvent_Create : IDemoEvent { }

    public class DemoEvent_Destroy : IDemoEvent { }

    public class DemoEvent_Connect : IDemoEvent { }

    public class DemoEvent_Disconnect : IDemoEvent { }
}