# 快速入门

>[!NOTE]
> 这是一个针对 MobileSuit 3+版本编写的新文档(使用依赖注入), 感谢 [@nianWu1](https://github.com/nianWu1) 的贡献。

HitRefresh.MobileSuit 是一个强大的工具，可以快速构建 .NET Core 控制台应用程序。

在本文中，我将介绍步步为营地教你从命令行构建基本的MobileSuit应用程序。

## 创建项目

首先，在你的 IDE 中创建一个新的应用程序，或者使用**.NET CLI**:

```pwsh
dotnet new console --framework <你需要的dotnet版本，例如net8> --use-program-main -o <你的项目名>
```

然后，通过 NuGet 包管理器 向项目添加[HitRefresh.MobileSuit](https://www.nuget.org/packages/HitRefresh.MobileSuit/) ，你也可以使用**.NET CLI**:

```pwsh
dotnet add package HitRefresh.MobileSuit
```

## 修改 `Program.cs`

>[!NOTE]
> 我们正在使用顶级语句，而无需显式的Main方法

清空所有的现存代码，并添加这些代码:

``` csharp
using HitRefresh.MobileSuit;

Suit.GetBuilder().Build<Client>().Run();
```

>[!NOTE]
> 这其中`Client`类是我们自己定义的客户端类，下一节我将讲解如何编写MobileSuit客户端

## 编写MobileSuit客户端类

### 创建类

在项目中添加名为Client的类，你可以为自己的应用程序添加名称，这里我添加Client类的属性定义[SuitInfo("Demo")]。
该Client类需要有构造函数如下：

```csharp
    public Client(IIOHub io)
    {
        IO = io;
    }
```

其中，IO需要自己定义，类型为IIOHub，限定get方法。它会自动地通过**依赖注入**完成初始化。

```csharp
private IIOHub IO {get;}
```

### 引入必要的命名空间

引入以下依赖:

```csharp
using System;
using System.Collections.Generic;
using HitRefresh.MobileSuit;
using HitRefresh.MobileSuit.UI;
using HitRefresh.MobileSuit.Core;
```

### 添加第一条指令

向类`Client`添加一个名为`SayHello`的方法，用`IO.WriteLine()`来代替`Console.WriteLine()`，同时，你可以用`SuitAlias("cname")`添加该方法的别名，并且用`SuitInfo("msg")`来为方法提供解释说明

例如：

```csharp
[SuitAlias("SayH")]
[SuitInfo("Say hello to msg")]
public void SayHello(string msg){
    IO.WriteLine("hello world "+ msg);
}
```

### Client.cs整体的代码

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


## 运行并测试你的应用程序

在IDE中按**F5**，或者在终端执行`dotnet run`以构建并运行你的应用程序。

运行成功后可以在左侧看到应用名"Demo"; 在控制台中，你可以输入：

1. **Help** 查看 **MobileSuit** 的帮助
2. **List** 或 **ls** 查看当前客户端可用的所有指令
3. **SayHello** **name**或 **SayH** **name**运行 **Client.SayHello(string msg)**
4. ***Exit** 退出程序

Mobile Suit 中的命令可以在多行中输入，当行的最后一个字符是 *%* 时。
可以在行格式 "^\s*#" 中添加注释。

例如：

```text
 #comment
he%
llo
```

等价于

```text
hello
```

构建这样一个应用程序的过程非常简单，你只需要编写一个类。
