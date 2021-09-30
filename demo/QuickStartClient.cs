using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class QuickStartClient 
    {
        public IIOHub IO { get; }

        public QuickStartClient(IIOHub io)
        {
            IO = io;
        }
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