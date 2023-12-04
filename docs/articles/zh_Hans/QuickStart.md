# 更快地开始

你可能先看了[快速入门](./GetStarted.md)然后跟着教程搞不明白，或者跑不起来。
这里我们更快，手把手教你从命令行构建基本的MobileSuit应用程序。

## 创建项目

首先，创建一个新的C#应用程序

```
dotnet new console --framework <你需要的dotnet版本，例如net7.0> --use-program-main -o <你的项目名>
```

然后进入项目目录，添加[HitRefresh.MobileSuit](https://www.nuget.org/packages/HitRefresh.MobileSuit/) ，这里我用的是net7.0，所以执行
```
dotnet add package PlasticMetal.MobileSuit --version 4.2.1
```

## 修改应用程序的主方法

在主方法中，即Program.cs中，添加
```
Suit.QuickStart4BitPowerLine<Client>(args);
```
并且引用
```
using PlasticMetal.MobileSuit;
```
将Program类放到命名空间PlasticMetal.MobileSuitDemo中，Program.cs修改结果如下：
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
其中Client类是我们自己定义的客户端类，接下来我门将讲解如何编写MobileSuit客户端

## 编写MobileSuit客户端类


### 创建类

在项目中添加名为Client的类，它需要位于命名空间PlasticMetal.MobileSuitDemo中，你可以为自己的应用程序添加名称，这里我添加Client类的属性定义[SuitInfo("Demo")]。
该Client类需要有构造函数如下：

```
    public Client(IIOHub io)
    {
        IO = io;
    }
```
其中，IO需要自己定义，类型为IIOHub，限定get方法。
### 添加引用的头文件

```
using System;
using System.Collections.Generic;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.UI;
using PlasticMetal.MobileSuit.Core;
```
最基本的，需要上面这些头文件，

### 添加第一条指令

向类Client添加一个名为SayHello的方法，用IO.WriteLine()来代替Console.WriteLine()，同时，你可以用SuitAlias("cname")添加该方法的别名，并且用SuitInfo("msg")来为方法提供解释说明

例如：
```
    [SuitAlias("SayH")]
    [SuitInfo("Say hello to msg")]
    public void SayHello(string msg){
        IO.WriteLine("hello world "+ msg);
    }
```

### Client.cs整体的代码

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

## 运行并且测试你的应用程序

### 运行程序

命令行直接使用
```
dotnet run
```

运行成功后可以在左侧看到应用名"Demo"
在控制台中，你可以输入:

1. **Help** 查看 **MobileSuit** 的帮助

2. **List** 或 **ls** 查看当前客户端可用的所有指令

3. **SayHello** **name**或 **SayH** **name**运行 **Client.SayHello(string msg)**

5. ***Exit** 退出程序
