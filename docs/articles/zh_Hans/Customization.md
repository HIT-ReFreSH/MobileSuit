# MobileSuit 的定制

> [!WARNING]
> 这是一个针对旧版本MobileSuit(v2.x,v1.x)编写的文档，很久没有更新，可能无法正常生效

MobileSuit通过配置HostBuilder支持许多定制方式。

## 定制设置和颜色

您可以将自己的设置变成 **HostSettings**，然后使用
`SuitHostBuilder.ConfigureSetting(HostSettings settings)` 来进行配置。

同样，可以使用 `SuitHostBuilder.ConfigureColor(ColorSetting setting)` 来配置颜色。

## 定制输入输出

`SuitHostBuilder.UseIO<T>()` 使得这成为可能。只需将 `T` 设为 IIOServer 的名称。

## 定制日志记录器

`SuitHostBuilder.UseLog(Logger logger)` 使得这成为可能。只需将 `logger` 设为 Logger。

for example:

``` csharp
return Suit.GetBuilder()
	.UseLog(ILogger.OfDirectory(@"D:\\"))
	.Build<Client>().Run(args);
```

## Customize BuildInCommand

`SuitHostBuilder.UseBuildInCommand<T>()` make it possible. Just make `T` the name of IBuildInCommandServer.

例如：

``` csharp
return Suit.GetBuilder()
	.UseBuildInCommand<DiagnosticBuildInCommandServer>()
	.Build<Client>().Run(args);
```

## 定制提示符

`SuitHostBuilder.UsePrompt<T>()` 使得这成为可能。只需将 `T` 设为 IPromptServer 的名称。

例如：

``` csharp
return Suit.GetBuilder()
	.UsePrompt<PowerLineThemedPromptServer>()
	.Build<Client>().Run(args);
```