# Quick Start

>[!NOTE]
> This is a newly-written documentation for version 3+ MobileSuit (with Dependency Injection), Thanks for the contributions made by [@nianWu1](https://github.com/nianWu1)

HitRefresh.MobileSuit is a powerful tool to quickly build a .NET Core ConsoleApp.

Here we are faster, step-by-step to walk you through building a basic MobileSuit application from the command line. In this article, I'll introduce to build a basic MobileSuit App.

## Create your project

So firstly, you need to create a new Application in your **IDE**, or use **.NET CLI**:

```pwsh
dotnet new console --framework <the dotnet version you need , e.g. net8> --use-program-main -o <The name of your project>
```

Then, go to the project directory, and add [HitRefresh.MobileSuit](https://www.nuget.org/packages/HitRefresh.MobileSuit/) to your project through **Nuget Package Manager** or **.NET CLI**:

```pwsh
dotnet add package HitRefresh.MobileSuit
```

## Modify `Program.cs`

>[!NOTE]
> We're using the top-class statements!

Clear all the existing code in this file, and then add the following code:

``` csharp
using HitRefresh.MobileSuit;

Suit.GetBuilder().Build<Client>().Run();
```

>[!NOTE]
> Here, the`Client` class is a client class that we define ourselves, and I will explain how to write a MobileSuit client

## Write the MobileSuit client class

### Create a class

Add a class called `Client` to your project, it needs to be located in the namespace HitRefresh.MobileSuitDemo, you can add a name for your application, here I add the property definition of the Client class `[SuitInfo("Demo")]`.
The client class needs to have the following constructor:

```csharp
public Client(IIOHub io)
{
    IO = io;
}
```

Among them, the I/O needs to be defined by itself, the type is `IIOHub`, and the get method is defined. This IIOHub will be initialized automatically by *Dependency Injection*.

```csharp
private IIOHub IO {get;}
```


### Using namespaces

```csharp
using System;
using System.Collections.Generic;
using HitRefresh.MobileSuit;
using HitRefresh.MobileSuit.UI;
using HitRefresh.MobileSuit.Core;
```

The most basic is the header file above

### Add the first directive

Add a method called SayHello to the client class, replace `Console.WriteLine()` with `IO.WriteLine()`, add the alias of the method with `SuitAlias("alias")`, and use `SuitInfo("msg")` to explain the method

For example：

```csharp
[SuitAlias("SayH")]
[SuitInfo("Say hello to msg")]
public void SayHello(string msg){
    IO.WriteLine("hello world "+ msg);
}
```

### Client.cs code as a whole

```csharp
using System;
using System.Collections.Generic;
using HitRefresh.MobileSuit;
using HitRefresh.MobileSuit.UI;
using HitRefresh.MobileSuit.Core;


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

Press **F5** in the IDE, or directly use from the command line:

```pwsh
dotnet run
```

After the operation is successful, you can see the application name "Demo" on the left.
In the console, you can enter:

1. **Help** to see help for MobileSuit
2. **List** or **ls** to see all available commands for current client.
3. **SayHello** **name** or **SayH** **name** to run **Client.SayHello(string msg)**
4. **Exit** to exit the progress

Commands in mobile suit can be input in multiline, when the last character in the line is *%*
Comments can be added in lines format "^\s*#"

For Example:

```text
 #comment
he%
llo
```

equals

```text
hello
```

The process of build such a app is so easy that you just need one class to write.
