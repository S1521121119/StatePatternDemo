using StatePatternDemo.DemoModule.Interfaces;
using System;

namespace StatePatternDemo.DemoModule
{
    public class Demo_Driven : IDemo_Driven
    {
        // Entry Action

        public void EntryNull() => Console.WriteLine("Execute EntryNull Process...");

        public void EntryIdle() => Console.WriteLine("Execute EntryIdle Process...");

        public void EntryReady() => Console.WriteLine("Execute EntryReady Process...");

        // Exit Action

        public void ExitNull() => Console.WriteLine("Execute ExitNull Process...");

        public void ExitIdle() => Console.WriteLine("Execute ExitIdle Process...");

        public void ExitReady() => Console.WriteLine("Execute ExitReady Process...");

        // Output Action

        public void ActionConnectProcess() => Console.WriteLine("Execute ConnectProcess Process...");

        public void ActionDisconnectProcess() => Console.WriteLine("Execute DisconnectProcess Process...");
    }
}