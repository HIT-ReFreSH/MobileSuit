using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace MobileSuitDemo
{
    public class Client : SuitClient
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
