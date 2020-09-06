---
title: Useful Features for PlasticMetal.JMobileSuitLite
date: 2020-04-19 12:45:11
---


## Avoid some public member show in Mobile Suit

Always there are some public methods in your client class, but if you want some of them not to show in mobile suit, add ***@SuitIgnore*** annotions to them.

## Multiline Input

Commands in mobile suit can be input in multiline, when the last character in the line is *%*

For Example:

``` MobileSuitScript
he%
llo
```

equals

``` MobileSuitScript
hello
```

## Customized Prompt server and Powerline Themed Prompt Server

You may write your own Prompt Server class, which implements interface ***PlasticMetal.JMobileSuitLite.IO.PromptServer***. Also, they can just extend class ***PlasticMetal.JMobileSuitLite.IO.CommonPromptServer***.

Use the server to initialize a ***SuitConfiguration***, then use the configuration to initialize a ***SuitHost***.

A customized theme: Powerline is built in Package PlasticMetal.JMobileSuitLite.

You may use it like:

``` java
new SuitHost(Client.class, 
                PowerLineThemedPromptServer.getPowerLineThemeConfiguration()).Run();
```

If garbled, Make sure you have installed and enabled [Powerline Fonts](https://github.com/powerline/fonts) for your Console.