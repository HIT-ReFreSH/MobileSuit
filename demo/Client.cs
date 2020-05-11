using System.Threading.Tasks;
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
            return $"bye, {IO.ReadLine("Name", "foo",true)}";
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
