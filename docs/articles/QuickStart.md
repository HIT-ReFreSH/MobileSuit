# Get started easier

You may have watched [Quickstart](./GetStarted.md) and then followed the tutorial and couldn't figure it out, or you couldn't run.
Here we are faster, step-by-step to walk you through building a basic MobileSuit application from the command line.

## Create a project

First, create a new C# application

```
dotnet new console --framework <the dotnet version you need , e.g. net7.0> --use-program-main -o <The name of your project>
```

Then go to the project directory and add [HitRefresh.MobileSuit](https://www.nuget.org/packages/HitRefresh.MobileSuit/), here I use net7.0, so execute
```
dotnet add package PlasticMetal.MobileSuit --version 4.2.1
```

## Modify the main function of the application

In the main method, in Program.cs, add
```
Suit.QuickStart4BitPowerLine<Client>(args);
```
And quote
```
using PlasticMetal.MobileSuit;
```
Put the Program class into the namespace PlasticMetal.MobileSuitDemo, and the Program.cs modification result is as follows:
```
using PlasticMetal.MobileSuit;
namespace PlasticMetal.MobileSuitDemo{
    class Program
    {
        public static void Main(string[] args)
        {
            Suit.QuickStart4BitPowerLine<Client>(args);
        }
    }
}
```
The Client class is a client class that we define ourselves, and I will explain how to write a MobileSuit client

## Write the MobileSuit client class


### Create a class

Add a class called Client to your project, it needs to be located in the namespace PlasticMetal.MobileSuitDemo, you can add a name for your application, here I add the property definition of the Client class [SuitInfo("Demo")].
The client class needs to have the following constructor:

```
    public Client(IIOHub io)
    {
        IO = io;
    }
```
Among them, the I/O needs to be defined by itself, the type is IIOHub, and the get method is defined.
### Add a referenced header file

```
using System;
using System.Collections.Generic;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.UI;
using PlasticMetal.MobileSuit.Core;
```
The most basic is the header file above

### Add the first directive

Add a method called SayHello to the client class, replace Console.WriteLine() with IO.WriteLine(), add the alias of the method with SuitAlias("cname"), and use SuitInfo("msg") to explain the method

For example：
```
    [SuitAlias("SayH")]
    [SuitInfo("Say hello to msg")]
    public void SayHello(string msg){
        IO.WriteLine("hello world "+ msg);
    }
```

### Client.cs code as a whole

```

using System;
using System.Collections.Generic;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.UI;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuitDemo;

[SuitInfo("Demo")]
public class Client
{
    /// <summary>
    ///     Initialize a client
    /// </summary>
    public Client(IIOHub io)
    {
        IO = io;
    }

    public IIOHub IO { get; }

    [SuitAlias("SayH")]
    [SuitInfo("Say hello to msg")]
    public void SayHello(string msg){
        IO.WriteLine("hello world "+ msg);
    }
}
```

## Run and test your application

### Run the program

Direct use from the command line
```
dotnet run
```

After the operation is successful, you can see the application name "Demo" on the left.
In the console, you can enter:


1. **Help** to see help for MobileSuit
2. **List** or **ls** to see all available commands for current client.
3. **SayHello** **name** or **SayH** **name** to run **Client.SayHello(string msg)**
5. **Exit** to exit the progress