---
title: Create Complex Commands - PlasticMetal.JMobileSuitLite
date: 2020-04-11 12:45:11
---

**These features only works on JMobileSuitLite 0.1.1.4 or later version**

Code example is at the end of this part.

## Parsable-type Parameters

**This feature only works on JMobileSuitLite 0.1.6 or later version**

MobileSuit's parameter type can be anything you like, in case there is a parser;
JMobileSuitLite support this function, now.

### Create or Select a parser

A parser is a method that takes a String as input, then output a object;
For example: Integer::parseInt, Long::parseLong, and so on.
Parsers must be **public static** modified. 

### Create a command with Parsable-type parameter

Add a ***SuitParser*** annotation to the parameter, which can tell the parser's name and Container Class

For example:

``` java
    @SuitAlias("Sn")
    public void ShowNumber(
            @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
            int i
    ){
        IO().WriteLine(String.valueOf(i));
    }
```

If the user inputs a illegal argument that can't be parsed, ***TraceBack*** will be **InvalidCommand**.



## Array parameter

### Create a command with only a array parameter

Add a method called ***GoodEvening*** to class ***Client*** . It has a ***String[]*** parameter. You may add a alias 'GE' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodEvening** with 0, 1, 2 ... arguments, which will be seen as an array. 

### Create a command with a array parameter and other parameters

Add a method called ***GoodEvening2*** to class ***Client*** . It has a ***String[]*** parameter, and a ***String*** parameter. The parameter with ***String[]*** type **MUST BE** the last parameter of the method.  You may add a alias 'GE2' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodEvening2** with 1, 2 ... arguments, first will be mapped to the ***String*** parameter, else will be seen as an array and mapped to the ***String[]*** one. 

The most important thing when you're using this type of command is that The parameter with ***String[]*** type **MUST BE** the last parameter of the method. If not, JMobileSuit will not parse you command correctly.

### Array parameter for Parsable-types

**This feature only works on JMobileSuitLite 0.1.6 or later version**

The array can not only be ***String[]***, but also other types. You just need to add a ***SuitParser*** Annotation before the array parameter.

For example:

```java
    @SuitAlias("Sn")
    public void ShowNumber(
            @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
            int i,
            @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
            int[] j
    ){
        IO().WriteLine(String.valueOf(i));
        IO().WriteLine(j.length>=1?String.valueOf(j[0]):"");
    }
```

**Attention!** The parser parsed String to something, **NOT** String[] to something!


## Dynamic Parameter

### Create a class implements DynamicParameter

Add a class to class ***Client***, called ***GoodMorningParameter***, it implements ***PlasticMetal.JMobileSuitLite.ObjectModel.Interfaces.DynamicParameter*** interface. The class should be `public static`:

``` java
    public static class GoodMorningParameter implements DynamicParameter{

        /**
         * Parse this Parameter from String[].
         *
         * @param options String[] to parse from.
         * @return Whether the parsing is successful
         */
        @Override
        public Boolean Parse(String[] options)
        {


        }
    }
```

Add some Contents to ***GoodMorningParameter*** , fill the ***Parse(String[] options)***. The method should return true, if parsing is successful, otherwise, it should return false.

For example:

``` java
    public static class GoodMorningParameter implements DynamicParameter{
        public String name="foo";

        /**
         * Parse this Parameter from String[].
         *
         * @param options String[] to parse from.
         * @return Whether the parsing is successful
         */
        @Override
        public Boolean Parse(String[] options)
        {
            if(options.length==1){
                name=options[0];
                return true;
            }else return options.length==0;

        }
    }
```

### Create a command with only a DynamicParameter

Add a method called ***GoodMorning*** to class ***Client*** . It has a ***GoodMorningParameter*** parameter. You may add a alias 'GM' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodMorning** with 0, 1, 2 ... arguments, which will be seen as an array, then parsed by ***GoodMorningParameter::Parse***. 

### Create a command with a DynamicParameter and other parameters

Add a method called ***GoodMorning2*** to class ***Client*** . It has a ***GoodMorningParameter*** parameter, and a ***String*** parameter. The parameter with ***GoodMorningParameter*** type **MUST BE** the last parameter of the method.  You may add a alias 'GE2' for this method. The content of method can be anything you like.

Build and run your application.

In the console, you may input:

**GoodMorning2** with 1, 2 ... arguments, first will be mapped to the ***String*** parameter, else will be seen as an array and parsed to the ***GoodMorningParameter*** one. 

The most important thing when you're using this type of command is that The parameter with ***? extends DynamicParameter*** type **MUST BE** the last parameter of the method. If not, JMobileSuit will not parse you command correctly.

### Auto DynamicParameter Class

**This feature only works on JMobileSuitLite 0.1.6 or later version**

Now, you no more needs to write a DynamicParameter::Parse yourself.

You just need to make the class extend ***AutoDynamicParameter***, add some Fields and add Annotation for the fields.

There are 4 Annotations can be added to fields:

1. ***Option*** means this is a option, that will be parsed by "-xxx value". If you want to parse "-xxx value_part1 value_part2", specific the Length of option like: `@Option(value="xxx",length=2)`; The input of parser will be "value_part1 value_part2"
2. ***Switch*** means this is a switch, This field must be ***boolean***,that will be parsed by "-sw". If "-sw" exists, this field is true; if not, The field's value **WILL NOT BE CHANGED**, but not be parsed as false. A field can only be one of ***Switch*** and ***Option***.
3. ***WithDefault*** means this field has a default value, so it don't have to be parsed. ***Switch*** field don't need this annotation.
4. ***SuitParser*** if a ***Option*** field's type is not String, use this annotation to specific the parser.

If a field has none of ***Switch*** or ***Option***, it will not be parsed.

If some ***Option*** fields without ***WithDefault*** is not parsed, the parsing is failed.

For Example:

``` java
    public static class SleepArgument extends AutoDynamicParameter{
        @Option("n")
        public String Name;

        @Option("t")
        @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
        @WithDefault
        public int SleepTime=0;
        @Switch("s")
        public boolean isSleeping;
    }
```

### Code example

After this part, your **Client.java** may looks like:

``` java
package PlasticMetal.JMobileSuit.Demo;

import PlasticMetal.JMobileSuitLite.NeuesProjekt.PowerLineThemedPromptServer;
import PlasticMetal.JMobileSuitLite.ObjectModel.Annotions.SuitAlias;
import PlasticMetal.JMobileSuitLite.ObjectModel.Annotions.SuitInfo;
import PlasticMetal.JMobileSuitLite.ObjectModel.DynamicParameter;
import PlasticMetal.JMobileSuitLite.ObjectModel.Parsing.*;
import PlasticMetal.JMobileSuitLite.ObjectModel.SuitClient;
import PlasticMetal.JMobileSuitLite.SuitHost;

@SuitInfo("Demo")
public class Client extends SuitClient
{
    @SuitAlias("H")
    @SuitInfo("hello command")
    public void Hello()
    {

        IO().WriteLine("Hello! MobileSuit!");
    }

    public String Bye(String name)
    {
        IO().WriteLine("Bye!" + name);
        return "bye";
    }

    public static void main(String[] args) throws Exception
    {
        new SuitHost(Client.class,
                PowerLineThemedPromptServer.getPowerLineThemeConfiguration()).Run();
    }
    @SuitAlias("GM")
    public void GoodMorning(GoodMorningParameter arg){
        IO().WriteLine("Good morning,"+arg.name);
    }

    @SuitAlias("GM2")
    public void GoodMorning2(String arg, GoodMorningParameter arg1){
        IO().WriteLine("Good morning, "+arg+" and "+ arg1.name);
    }

    @SuitAlias("GE")
    public void GoodEvening(String[] arg){

        IO().WriteLine("Good Evening, "+(arg.length>=1?arg[0]:""));
    }

    @SuitAlias("Sn")
    public void ShowNumber(
            @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
            int i,
            @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
            int[] j
    ){
        IO().WriteLine(String.valueOf(i));
        IO().WriteLine(j.length>=1?String.valueOf(j[0]):"");
    }

    @SuitAlias("GE2")
    public void GoodEvening2(String arg0,String[] arg){

        IO().WriteLine("Good Evening, "+arg0+(arg.length>=1?" and "+arg[0]:""));
    }

    @SuitAlias("Sl")
    @SuitInfo("Sleep {-n name (, -t hours, -s)}")
    public void Sleep(SleepArgument argument){
        if(argument.isSleeping){
            IO().WriteLine(argument.Name+" has been sleeping for "+ argument.SleepTime +" hour(s)." );
        }else {
            IO().WriteLine(argument.Name+" is not sleeping." );
        }
    }

    public static class SleepArgument extends AutoDynamicParameter{
        @Option("n")
        public String Name;

        @SuppressWarnings("CanBeFinal")
        @Option("t")
        @SuitParser(ParserClass = Integer.class, MethodName = "parseInt")
        @WithDefault
        public int SleepTime=0;
        @Switch("s")
        public boolean isSleeping;
    }

    public static class GoodMorningParameter implements DynamicParameter{
        public String name="foo";

        /**
         * Parse this Parameter from String[].
         *
         * @param options String[] to parse from.
         * @return Whether the parsing is successful
         */
        @Override
        public boolean Parse(String[] options)
        {
            if(options.length==1){
                name=options[0];
                return true;
            }else return options.length==0;

        }
    }
}
```