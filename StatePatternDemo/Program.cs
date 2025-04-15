using StatePatternDemo.DemoModule;
using StatePatternDemo.DemoModule.Interfaces;

namespace StatePatternDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDemo_Driven Demo_Driven = new Demo_Driven();
            IDemo_Context Demo_Context = new Demo_Context(Demo_Driven);
            Demo_Context.HandleEvent(new Demo_Event_Create());
            Demo_Context.HandleEvent(new Demo_Event_Connect());
            Demo_Context.HandleEvent(new Demo_Event_Disconnect());
            Demo_Context.HandleEvent(new Demo_Event_Destroy());
        }
    }
}