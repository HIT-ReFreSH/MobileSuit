<div  align=center>
    <img src="https://raw.githubusercontent.com/HIT-ReFreSH/MobileSuit/main/images/logo.png" width = 30% height = 30%  />
</div>

# HitRefresh.MobileSuit

![nuget](https://img.shields.io/nuget/v/HitRefresh.MobileSuit?style=flat-square)
![Nuget](https://img.shields.io/nuget/dt/HitRefresh.MobileSuit?style=flat-square)
![GitHub](https://img.shields.io/github/license/HIT-ReFreSH/MobileSuit?style=flat-square)
![GitHub last commit](https://img.shields.io/github/last-commit/HIT-ReFreSH/MobileSuit?style=flat-square)
![Test & Publish](https://img.shields.io/github/actions/workflow/status/HIT-ReFreSH/MobileSuit/publish.yml?style=flat-square&label=Test%20%26%20Publish)
![Documentation Publish](https://img.shields.io/github/actions/workflow/status/HIT-ReFreSH/MobileSuit/docs.yml?style=flat-square&label=Documentation%20Publish)
![GitHub repo size](https://img.shields.io/github/repo-size/HIT-ReFreSH/MobileSuit?style=flat-square)
![GitHub code size](https://img.shields.io/github/languages/code-size/HIT-ReFreSH/MobileSuit?style=flat-square)

[View at Nuget.org](https://www.nuget.org/packages/HitRefresh.MobileSuit/)

[View Documentation](https://HIT-ReFreSH.github.io/MobileSuit/articles/intro.html) | [查看中文文档](https://HIT-ReFreSH.github.io/MobileSuit/articles/zh_Hans/intro.html)

MobileSuit provides an easy way to quickly build a .NET Console App.

Focus on writing the backend part, Import HitRefresh.MobileSuit, and simply write a Frontend in a very simple standard,
then a beautiful Console App is born.

For Example: HIT-Schedule-Master CLI

![MsRtExample-1](https://raw.githubusercontent.com/HIT-ReFreSH/MobileSuit/main/images/MsRtExample-1.png)

with PowerLine theme & I18N support

![MsRtExample-2](https://raw.githubusercontent.com/HIT-ReFreSH/MobileSuit/main/images/MsRtExample-2.png)

**Previously named as [PlasticMetal.MobileSuit](https://github.com/Plastic-Metal/MobileSuit).**



```markdown
## Theme System

MobileSuit now supports several popular color themes, including Nord, Dracula, Solarized Light/Dark, and Monokai.

### Available Themes

| Theme | Description | Screenshot |
|-------|-------------|------------|
| **Nord** | Arctic, north-bluish color palette | ![Nord Theme](docs/images/themes/nord.png) |
| **Dracula** | Popular dark theme | ![Dracula Theme](docs/images/themes/dracula.png) |
| **Solarized Light** | Light Solarized theme |
| **Solarized Dark** | Dark Solarized theme | ![Solarized Dark](docs/images/solarized.png) |
| **Monokai** | Sublime Text style theme | 
| **Default** | System default theme | |

### Usage

#### Set theme at startup

```csharp
var host = Suit.CreateBuilder(args)
    .UseNordTheme()            // Use Nord theme
    .MapClient<Client>()
    .Build();