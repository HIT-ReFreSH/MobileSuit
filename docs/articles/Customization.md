# Customization for MobileSuit

>![WARNING]
> This is documentation written for old MobileSuit(v2.x,v1.x), so this may not work in the new versions.

MobileSuit supports customization in Many ways through configuring the HostBuilder.

## Customize Settings & Color

You can make your own setting into an **HostSettings**, then use 
`SuitHostBuilder.ConfigureSetting(HostSettings settings)` to Configure it.

Similarly, use `SuitHostBuilder.ConfigureColor(ColorSetting setting)` to Configure color.

## Customize IO

`SuitHostBuilder.UseIO<T>()` make it possible. Just make `T` the name of IIOServer.

## Customize Logger

`SuitHostBuilder.UseLog(Logger logger)` make it possible. Just make `logger` the Logger.

for example:

``` csharp
            return Suit.GetBuilder()
                .UseLog(ILogger.OfDirectory(@"D:\\"))
                .Build<Client>().Run(args);
```

## Customize BuildInCommand

`SuitHostBuilder.UseBuildInCommand<T>()` make it possible. Just make `T` the name of IBuildInCommandServer.

for example:

``` csharp
            return Suit.GetBuilder()
                .UseBuildInCommand<DiagnosticBuildInCommandServer>()
                .Build<Client>().Run(args);
```

## Customize Prompt

`SuitHostBuilder.UsePrompt<T>()` make it possible. Just make `T` the name of IPromptServer.

for example:

``` csharp
            return Suit.GetBuilder()
                .UsePrompt<PowerLineThemedPromptServer>()
                .Build<Client>().Run(args);
```