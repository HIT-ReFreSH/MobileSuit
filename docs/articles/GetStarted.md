# Quick Start

HitRefresh.MobileSuit is a powerful tool to quickly build a .NET Core ConsoleApp.

In this article, I'll introduce to build a basic MobileSuit App.

## Create your project

So firstly, you need to create a new Application in your IDE.

Then, add [HitRefresh.MobileSuit](https://www.nuget.org/packages/HitRefresh.MobileSuit/) to your project through nuget.

## Write the MobileSuit Client class

### Create the class

Add a Class to your project, named ***QuickStartClient*** . It inherits class ***HitRefresh.MobileSuit.ObjectModel.SuitClient*** .

### Add custom attributes to the class

*HitRefresh.MobileSuit.SuitInfoAttribute* with argument "Demo"

### Add the first command

Add a method called ***Hello*** to class ***Client*** . It has no parameters and return value.

 The content of method can be anything you like. You can use *IO.WriteLine* and *IO.ReadLine* instead of *Console.WriteLine* and *Console.ReadLine*.

### Add information and Alias for the first command

Add custom attributes to the method:

1. *HitRefresh.MobileSuit.SuitInfoAttribute* with argument "hello command."
2. *HitRefresh.MobileSuit.SuitAliasAttribute* with argument "H"

### Add another command

Add a method called ***Bye*** to class ***Client***. It has a string parameter, named *name*. It returns a *string*.

 The content of method can be anything you like. You can use *IO.WriteLine* and *IO.ReadLine* instead of *Console.WriteLine* and *Console.ReadLine*.

### Additionally

If you don't want some public methods to appear in mobile suit, Add ***SuitIgnore*** Attribute to it.

## Modify main method of your application

In the main method

``` csharp
public static void Main(String[] args){
    ...
}
```

add the following code:

``` csharp
Suit.GetBuilder().Build<QuickStartClient>().Run();
```

## Check your code for QuickStartClient.cs

It may looks like:

``` csharp
using HitRefresh.MobileSuit;
using HitRefresh.MobileSuit.ObjectModel;

namespace HitRefresh.MobileSuitDemo
{
    [SuitInfo("Demo")]
    public class QuickStartClient : SuitClient
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


```

## Run and test your Application

Build and run your application.

In the console, you may input:

1. **Help** to see help for MobileSuit
2. **List** or **ls** to see all available commands for current client.
3. **Hello** or **h** to run *QuickStartClient.Hello()*
4. **Bye** ***name*** to run *QuickStartClient.Bye(* ***name*** *)*
5. **Exit** to exit the progress

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

To let your application accept string[] args, See [Create CommandLine Application](./CreateCommandLineApplication.md)
