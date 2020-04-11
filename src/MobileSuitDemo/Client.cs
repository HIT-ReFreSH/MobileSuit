using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace MobileSuitDemo
{
    public class Client : MsClient
    {
        [MsAlias("H")]
        [MsInfo("hello command.")]
        public void Hello()
        {
            Io.WriteLine("Hello! MobileSuit!");
        }

        public string Bye(string name)
        {
            Io.WriteLine($"Bye! {name}");
            return "bye";
        }
    }
}
