using StatePatternDemo.DemoModule;
using StatePatternDemo.DemoModule.Interfaces;

namespace StatePatternDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDemoDriven demoDriven = new DemoDriven();
            IDemoContext demoContext = new DemoContext(demoDriven);
            demoContext.HandleEvent(new DemoEvent_Create());
            demoContext.HandleEvent(new DemoEvent_Connect());
            demoContext.HandleEvent(new DemoEvent_Disconnect());
            demoContext.HandleEvent(new DemoEvent_Destroy());
        }
    }
}