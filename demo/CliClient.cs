using PlasticMetal.MobileSuit;

namespace PlasticMetal.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class CliClient : MobileSuit.ObjectModel.CommandLineApplication
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
        public override int SuitStartUp(string[] args)
        {
            IO.WriteLine(args[0]);
            return 0;
        }
    }
}
