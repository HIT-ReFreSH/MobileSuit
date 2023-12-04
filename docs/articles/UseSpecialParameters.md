# Use Special Parameters in Commands

> [!WARNING]
> This is documentation written for old MobileSuit(v2.x,v1.x), so this may not work in the new versions.

## Parsable-type Parameters

MobileSuit's parameter type can be anything you like, in case there is a parser.

### Create or Select a parser

A parser is a method that takes a String as input, then output a object;
For example:

```csharp
public static object NumberConvert(string arg) => int.Parse(arg);
```

Parsers must be **public static** modified, returning **object** and taking a **string** as input. 

### Create a command with Parsable-type parameter

Add a ***SuitParser*** attribute to the parameter, which can tell the parser's name and Container Class

For example:

``` csharp
[SuitAlias("Sn")]
public void ShowNumber([SuitParser(typeof(Client),nameof(NumberConvert))]int i)
{
    IO.WriteLine(i.ToString());
}
```

If the user inputs a illegal argument that can't be parsed, ***TraceBack*** will be **InvalidCommand**.

## Array parameter

### Create a command with only a array parameter

Add a method called ***GoodEvening*** . It has a ***String[]*** parameter. You may add a alias 'GE' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodEvening** with 0, 1, 2 ... arguments, which will be seen as an array. 

Code:

``` csharp
[SuitAlias("GE")]
public void GoodEvening(string[] arg)
{

    IO.WriteLine("Good Evening, " + (arg.Length >= 1 ? arg[0] : ""));
}
```

### Create a command with a array parameter and other parameters

Add a method called ***GoodEvening2*** . It has a ***string[]*** parameter, and a ***string*** parameter. The parameter with ***string[]*** type **MUST BE** the last parameter of the method.  You may add a alias 'GE2' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodEvening2** with 1, 2 ... arguments, first will be mapped to the ***string*** parameter, else will be seen as an array and mapped to the ***string[]*** one. 

The most important thing when you're using this type of command is that The parameter with ***string[]*** type **MUST BE** the last parameter of the method. If not, JMobileSuit will not parse you command correctly.

Code:

``` csharp
[SuitAlias("GE2")]
public void GoodEvening2(string arg0, string[] args)
{

    IO.WriteLine("Good Evening, " + arg0 + (args.Length >= 1 ? " and " + args[0] : ""));
}
```

### Array parameter for Parsable-types

The array can not only be ***string[]***, but also other types. You just need to add a ***SuitParser*** before the array parameter.

For example:

```csharp
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
```

**Attention!** The parser parsed *string* to something, **NOT** *string[]* to something!

## Dynamic Parameter

### Create a class implements DynamicParameter

Add a class, called ***GoodMorningParameter***, it implements ***HitRefresh.MobileSuit.ObjectModel.IDynamicParameter*** interface. The class should be `public`:

Add some Contents to ***GoodMorningParameter*** , fill the ***Parse(string[]? options)***. The method should return true, if parsing is successful, otherwise, it should return false.

For example:

``` csharp
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
```

### Create a command with only a DynamicParameter

Add a method called ***GoodMorning*** to class ***Client*** . It has a ***GoodMorningParameter*** parameter. You may add a alias 'GM' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodMorning** with 0, 1, 2 ... arguments, which will be seen as an array, then parsed by ***GoodMorningParameter::Parse***. 

Code:

```csharp
[SuitAlias("GM")]
public void GoodMorning(GoodMorningParameter arg)
{
    IO.WriteLine("Good morning," + arg.name);
}
```

### Create a command with a DynamicParameter and other parameters

Add a method called ***GoodMorning2*** to class ***Client*** . It has a ***GoodMorningParameter*** parameter, and a ***String*** parameter. The parameter with ***GoodMorningParameter*** type **MUST BE** the last parameter of the method.  You may add a alias 'GE2' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodMorning2** with 1, 2 ... arguments, first will be mapped to the ***String*** parameter, else will be seen as an array and parsed to the ***GoodMorningParameter*** one.

The most important thing when you're using this type of command is that The parameter with ***? extends DynamicParameter*** type **MUST BE** the last parameter of the method. If not, MobileSuit will not parse you command correctly.

```csharp
[SuitAlias("GM2")]
public void GoodMorning2(string arg, GoodMorningParameter arg1)
{
    IO.WriteLine("Good morning, " + arg + " and " + arg1.name);
}
```

### Auto DynamicParameter Class

Normally, you do not need to write a IDynamicParameter.Parse yourself.

You just need to make the class extend ***AutoDynamicParameter***, add some properties and add Annotation for the properties.

There are 5 Attributess can be added to properties:

1. ***Option*** means this is a option, that will be parsed by "-xxx value". If you want to parse "-xxx value_part1 value_part2", specific the Length of option like: `[Option("xxx",2)]`; The input of parser will be "value_part1 value_part2"
2. ***Switch*** means this is a switch, This property must be ***boolean***,that will be parsed by "-sw". If "-sw" exists, this property is true; if not, The property's value **WILL NOT BE CHANGED**, but not be parsed as false. A property can only be one of ***Switch*** and ***Option***.
3. ***WithDefault*** means this property has a default value, so it don't have to be filled. Each time this property's called, the member will be added to the Collection. This property needs initialize. ***Switch*** property don't need this annotation.
4. ***AsCollection*** means this property is a collection, . ***Switch*** property don't need this annotation.
5. ***SuitParser*** if a ***Option*** property's type is not String, use this annotation to specific the parser.

If a property has none of ***Switch*** or ***Option***, it will not be parsed.

If some ***Option*** properties without ***WithDefault*** is not parsed, the parsing is failed.

For Example:

``` csharp
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
```

## Dynamic Parameters for CommandLine Application arg

We have learned to use string[] as CommandLine Application args in [Create CommandLine Application](./CreateCommandLineApplication.md), but how if I want to use a Dynamic Parameter as the arg?

Just make your client class extend CommandLineApplication\<TArgument\>, where TArgument is the Dynamic Parameter.