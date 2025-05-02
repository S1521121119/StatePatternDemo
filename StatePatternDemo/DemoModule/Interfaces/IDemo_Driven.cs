namespace StatePatternDemo.DemoModule.Interfaces
{
    public interface IDemo_Driven
    {
        // Entry Action

        void EntryNull();

        void EntryIdle();

        void EntryReady();

        // Exit Action

        void ExitNull();

        void ExitIdle();

        void ExitReady();

        // Output Action

        void ActionConnectProcess();

        void ActionDisconnectProcess();
    }
}