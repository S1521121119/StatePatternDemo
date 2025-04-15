using StatePatternDemo.DemoModule.Interfaces;
using System;

namespace StatePatternDemo.DemoModule
{
    public class Demo_Driven : IDemo_Driven
    {
        public void ActionConnectProcess() => Console.WriteLine("Execute ConnectProcess Process...");
        public void ActionDisconnectProcess() => Console.WriteLine("Execute DisconnectProcess Process...");
    }
}