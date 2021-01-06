using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class QuickStartClient : SuitClient
    {
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