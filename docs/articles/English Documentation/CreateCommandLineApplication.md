# Create CommandLine Application

Normally, CommandLine Application should accept a **string[] args** parameter, and return an int.

We assume that you have already read [Get Started](./GetStarted.md).

## Add a client class

Add a Class to your project, named ***CliClient*** . It inherits class ***HitRefresh.MobileSuit.ObjectModel.CommandLineApplication*** .

Then, override **int SuitStartUp(string[] args)**. This method will be called when args.Length>0 && args as a command cannot be resolved by MobileSuit.

## Add members and attributes

Do just like with ***QuickStartClient***.

## Modify main method of your application

In the main method

``` csharp
public static int Main(String[] args){
    ...
}
```

add the following code at the last line:

``` csharp
return Suit.GetBuilder().Build<QuickStartClient>().Run(args);
```

## Check your code for QuickStartClient.cs

It may looks like:

``` csharp
using HitRefresh.MobileSuit;

namespace HitRefresh.MobileSuitDemo
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



```

## Run and test your Application

Build your application. Run it with terminal, with shell commands like:

``` shell
demo hello
demo foo
demo bye bar
```

See what happens!