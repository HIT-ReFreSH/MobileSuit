using System.Threading.Tasks;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Parsing;

namespace PlasticMetal.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class Client : SuitClient
    {
        public class SleepArgument : AutoDynamicParameter
        {
            [Option("n")]
            public string Name{ get; protected set; }
            [Switch("s")]
            public bool IsSleeping{ get; protected set; }
        }

        /// <summary>
        ///     Initialize a client
        /// </summary>
        public Client()
        {
            Text = "Demo";
        }

        [SuitAlias("H")]
        [SuitInfo("hello command.")]
        public void Hello()
        {
            IO.WriteLine("Hello! MobileSuit!");
        }
        
        [SuitAlias("Sl")]
        public void Sleep(SleepArgument sleep)
        {
            IO.WriteLine(sleep.Name + (sleep.IsSleeping ? " is" : " is not") + " sleeping!");
        }

       
        public static object NumberConvert(string arg) => int.Parse(arg);

        public void Number([SuitParser(typeof(Client),nameof(NumberConvert))]int i)
        {
            IO.WriteLine(i.ToString());
        }

        public string Bye(string name)
        {
            IO.WriteLine($"Bye! {name}");
            return "bye";
        }

        public string Bye()
        {
            ;
            return $"bye, {IO.ReadLine("Name", "foo", true)}";
        }

        public async Task<string> HelloAsync()
        {
            await Task.Delay(10);
            return "Hello from async Task<string>!";
        }

        public async Task<string> HelloAsync(string name)
        {
            await Task.Delay(10);
            return $"Hello, {name}, from async Task<string>!";
        }
    }
}