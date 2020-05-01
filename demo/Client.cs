using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class Client : SuitClient
    {
        /// <summary>
        /// Initialize a client
        /// </summary>
        public Client():base()
        {
            Text = "Demo";
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

        public string Bye()
        {
            ;
            return $"bye, {IO.ReadLine("Name", "foo")}";
        }
    }
}
