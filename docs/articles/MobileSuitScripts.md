---
title: Pack-up your commands into MobileSuitScript Files - PlasticMetal.JMobileSuitLite
date: 2020-04-11 12:45:11
---

**This feature only works on JMobileSuitLite 0.1.2 or later version**

Pack-up your commands into a script file can make workflow automatically.

In this part, you need no Java codes to write. In stead, you should write a script file contains commands of your application.

## Create Script File

Create a text file. Filename dosen't matter, but we recommend you to make the extension ".mss" stands for MobileSuitScript.

Fill the file with commands of your application. **For JMobileSuitLite 0.1.2.2 or later version**, you can add comment lines starting with '#', which will not be parsed by JMobileSuit. 

The commands in the file may be invalid or not in the application (Object/Member not found), however, when JMobileSuit executed to the line which contains a command that execution result is not AllOk, the JMobileSuit will stop and notify the user, then the rest commands in the file **WILL NOT** be executed.

For example:

``` MobileSuitScript
#Will not be executed
#hello
hello
bye foo
#Stop at next line
@hello
#Will not be executed
@ls
```

## Run Script file in Application

Use *RunScript* or *Rs* command to run the script file. The argument is path (absolute or relative) of the file.

A ScriptFile may include a *RunScript* or *Rs* command, so watch out Stack Overflow.