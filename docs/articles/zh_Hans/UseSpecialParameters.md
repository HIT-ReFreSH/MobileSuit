# 在命令中使用特殊参数

>![WARNING]
> 这是一个针对旧版本MobileSuit(v2.x,v1.x)编写的文档，很久没有更新，可能无法正常生效

## 可解析类型参数

只要有解析器，MobileSuit 的参数类型可以是你喜欢的任何类型。

### 创建或选择解析器

解析器是一个接受字符串输入，然后输出对象的方法；
例如：

```csharp
public static object NumberConvert(string arg) => int.Parse(arg);
```
解析器必须是 **public static** 修改的，返回 **object**，以及以 **string** 为输入。

### 创建带有可解析类型参数的命令

向参数添加 ***SuitParser*** 属性，该属性可以告诉解析器的名称和容器类。

例如：

``` csharp
[SuitAlias("Sn")]
public void ShowNumber([SuitParser(typeof(Client),nameof(NumberConvert))]int i)
{
    IO.WriteLine(i.ToString());
}
```

如果用户输入无法解析的非法参数，***TraceBack*** 将为 **InvalidCommand**。

## 数组参数

### 创建仅带有数组参数的命令

添加一个名为 ***GoodEvening*** 的方法。它有一个 ***String[]*** 参数。你可以为此方法添加一个别名 'GE'。方法的内容可以是任意的。

构建并运行你的应用程序。

在控制台中，你可以输入：

**GoodEvening**，带有 0、1、2 或多个参数，这将被视为一个数组。

代码如下：

``` csharp
[SuitAlias("GE")]
public void GoodEvening(string[] arg)
{

    IO.WriteLine("Good Evening, " + (arg.Length >= 1 ? arg[0] : ""));
}
```

### 创建具有数组参数和其他参数的命令

添加一个名为 ***GoodEvening2*** 的方法。它有一个 ***string[]*** 参数和一个 ***string*** 参数。***string[]*** 类型的参数**必须**是方法的最后一个参数。你可以为此方法添加一个别名 'GE2'。方法的内容可以是任意的。

构建并运行你的应用程序。

在控制台中，你可以输入：

**GoodEvening2**，带有 1、2 或多个参数，第一个将被映射到 _**string**_ 参数，其他将被视为数组并映射到 _**string[]**_ 参数。

当使用这种类型的命令时，最重要的是 _**string[]**_ 类型的参数**必须**是方法的最后一个参数。如果不是，JMobileSuit 将无法正确解析你的命令。

代码：

``` csharp
[SuitAlias("GE2")]
public void GoodEvening2(string arg0, string[] args)
{

    IO.WriteLine("Good Evening, " + arg0 + (args.Length >= 1 ? " and " + args[0] : ""));
}
```

### 可解析类型的数组参数

数组不仅可以是 ***string[]***，还可以是其他类型。你只需在数组参数之前添加 ***SuitParser***。

例如：

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

**注意！** 解析器将 *string* 解析为某种类型，**而不是** *string[]*！


## 动态参数

### 创建实现 DynamicParameter 的类

添加一个名为 ***GoodMorningParameter*** 的类，它实现了 ***HitRefresh.MobileSuit.ObjectModel.IDynamicParameter*** 接口。该类应该是 `public` 的：

为 ***GoodMorningParameter*** 添加一些内容，填写 ***Parse(string[]? options)***。如果解析成功，该方法应返回 true；否则，它应返回 false。

例如：

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

### 创建仅带有 DynamicParameter 的命令

向类 ***Client*** 添加一个名为 ***GoodMorning*** 的方法。它有一个 ***GoodMorningParameter*** 参数。你可以为此方法添加一个别名 'GM'。方法的内容可以是任意的。

构建并运行你的应用程序。

在控制台中，你可以输入：

***GoodMorning***，带有 0、1、2 等参数，这将被视为一个数组，然后由 ***GoodMorningParameter::Parse*** 解析。

代码：

```csharp
[SuitAlias("GM")]
public void GoodMorning(GoodMorningParameter arg)
{
    IO.WriteLine("Good morning," + arg.name);
}
```

### 创建具有 DynamicParameter 和其他参数的命令

向类 ***Client*** 添加一个名为 ***GoodMorning2*** 的方法。它有一个 ***GoodMorningParameter*** 参数和一个 ***String*** 参数。参数的类型为 ***GoodMorningParameter*** 且**必须**放在方法的最后一个位置。你可以为此方法添加一个别名 'GE2'。该方法的内容可以是你喜欢的任何内容。

构建并运行你的应用程序。

在控制台中，你可以输入：

**GoodMorning2**，带有1、2或多个参数，第一个参数将被映射到 ***String*** 参数，其他参数将被视为数组并解析为 ***GoodMorningParameter***。

当你使用这种类型的命令时，最重要的是，类型为 ***? extends DynamicParameter*** 的参数**必须**放在方法的最后一个位置。如果不是这样，MobileSuit 将无法正确解析你的命令。

```csharp
[SuitAlias("GM2")]
public void GoodMorning2(string arg, GoodMorningParameter arg1)
{
    IO.WriteLine("Good morning, " + arg + " and " + arg1.name);
}
```

### 自动 DynamicParameter 类

通常情况下，你不需要自己编写 IDynamicParameter.Parse。

你只需让类扩展 ***AutoDynamicParameter***，添加一些属性并为属性添加注解。

可以向属性添加的 5 种属性：

1. ***Option*** 表示这是一个选项，将通过 "-xxx value" 进行解析。如果要解析 "-xxx value_part1 value_part2"，请指定选项的长度，如：`[Option("xxx", 2)]`；解析器的输入将为 "value_part1 value_part2"
2. **Switch** 表示这是一个开关，该属性必须为 ***boolean***，将通过 "-sw" 进行解析。如果存在 "-sw"，则该属性为 true；如果不存在，则该属性的值将**不会更改**，也不会解析为 false。一个属性只能是 ***Switch*** 或 ***Option*** 中的一个。
3. ***WithDefault*** 表示此属性具有默认值，因此无需填充。每次调用此属性时，成员将添加到集合中。此属性需要初始化。***Switch*** 属性不需要此注解。
4. ***AsCollection*** 表示此属性是一个集合。***Switch*** 属性不需要此注解。
5. ***SuitParser*** 如果 ***Option*** 属性的类型不是 String，请使用此注解来指定解析器。

如果一个属性既不是 ***Switch*** 也不是 ***Option***，则不会解析该属性。

如果一些 ***Option*** 属性没有 ***WithDefault*** 而未被解析，则解析失败。

例如：

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

## 命令行应用程序参数的动态参数

我们已经学会在[创建命令行应用程序](./CreateCommandLineApplication.md)中使用 string[] 作为命令行应用程序参数，但是如果我想将动态参数用作参数怎么办？

只需让你的客户端类扩展 CommandLineApplication\<TArgument>，其中 TArgument 是动态参数。