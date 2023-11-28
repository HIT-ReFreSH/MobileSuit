using HitRefresh.MobileSuit;

namespace HitRefresh.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class QuickStartClient
    {
        public QuickStartClient(IIOHub io)
        {
            IO = io;
        }

        public IIOHub IO { get; }

        [SuitAlias("H")]
        [SuitInfo("hello command.")]
        public void Hello()
        {
            IO.WriteLine("Hello! MobileSuit!");
        }


        public string Bye(string name)
        {
            IO.WriteLine($"Bye! {name}");
            return "bye";
        }
    }
}