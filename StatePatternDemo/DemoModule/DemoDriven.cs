using StatePatternDemo.DemoModule.Interfaces;
using System;

namespace StatePatternDemo.DemoModule
{
    public class DemoDriven : IDemoDriven
    {
        public void ActionConnectProcess()
        {
            Console.WriteLine("執行連線操作...");
        }

        public void ActionDisconnectProcess()
        {
            Console.WriteLine("執行斷線操作...");
        }
    }
}