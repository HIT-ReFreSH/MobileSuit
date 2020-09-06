using System.Collections.Generic;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Parsing;

namespace PlasticMetal.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class Client : SuitClient
    {


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
        [SuitInfo("Sleep {-n name (, -t hours, -s)}")]
        public void Sleep(SleepArgument argument)
        {
            var nameChain = "";
            foreach (var item in argument.Name)
            {
                nameChain += item;
            }
            if (nameChain == "") nameChain = "No one";

            if (argument.isSleeping)
            {
                IO.WriteLine(nameChain + " has been sleeping for " + argument.SleepTime + " hour(s).");
            }
            else
            {
                IO.WriteLine(nameChain + " is not sleeping.");
            }
        }

        public class SleepArgument : AutoDynamicParameter
        {
            [Option("n")]
            [AsCollection]
            [WithDefault]
            public List<string> Name { get; set; } = new List<string>();

            [Option("t")]
            [SuitParser(typeof(Client), nameof(NumberConvert))]
            [WithDefault]
            public int SleepTime { get; set; } = 0;
            [Switch("s")]
            public bool isSleeping { get; set; }
        }



        public static object NumberConvert(string arg) => int.Parse(arg);
        [SuitAlias("Sn")]
        public void ShowNumber([SuitParser(typeof(Client), nameof(NumberConvert))] int i)
        {
            IO.WriteLine(i.ToString());
        }
        [SuitAlias("Sn2")]
        public void ShowNumber2(
            [SuitParser(typeof(Client), nameof(NumberConvert))]
            int i,
            [SuitParser(typeof(Client), nameof(NumberConvert))]
            int[] j
    )
        {
            IO.WriteLine(i.ToString());
            IO.WriteLine(j.Length >= 1 ? j[0].ToString() : "");
        }
        [SuitAlias("GE")]
        public void GoodEvening(string[] arg)
        {

            IO.WriteLine("Good Evening, " + (arg.Length >= 1 ? arg[0] : ""));
        }

        [SuitAlias("GE2")]
        public void GoodEvening2(string arg0, string[] args)
        {

            IO.WriteLine("Good Evening, " + arg0 + (args.Length >= 1 ? " and " + args[0] : ""));
        }
        public class GoodMorningParameter : IDynamicParameter
        {
            public string name = "foo";

            /**
             * Parse this Parameter from String[].
             *
             * @param options String[] to parse from.
             * @return Whether the parsing is successful
             */

            public bool Parse(string[] options)
            {
                if (options.Length == 1)
                {
                    name = options[0];
                    return true;
                }
                else return options.Length == 0;

            }
        }
        [SuitAlias("GM")]
        public void GoodMorning(GoodMorningParameter arg)
        {
            IO.WriteLine("Good morning," + arg.name);
        }

        [SuitAlias("GM2")]
        public void GoodMorning2(string arg, GoodMorningParameter arg1)
        {
            IO.WriteLine("Good morning, " + arg + " and " + arg1.name);
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