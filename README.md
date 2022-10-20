# 说明
The web framework with best practices base on ASP.NET Core.

基于`ASP.NET Core`和`Entity Framework Core`的快速开发模板，提供一个规范化的项目目录及工程结构。

集成了`ater.droplet.cli`代码生成工具，帮助你生成基础代码，减少重复性的代码编写工作。

# 版本
|版本|.NET版本|
|-|-|
|6.x|.NET6|
|7.x|.NET7|

# 下载安装
## 使用源代码安装
- 拉取源代码
- 执行`install.ps1`脚本安装。

## 下载模板
模板已经发布到[`nuget`](https://www.nuget.org/packages/ater.web.templates)上，请根据你的项目版本下载对应的模板。

```pwsh
dotnet new --install ater.web.templates
```

## 创建项目
```pwsh
dotnet new atapi  
```
or
```pwsh
dotnet new atapi -n <projectname>
```
## 数据迁移
在项目`src\Database\EntityFramework.Migrator`目录下，执行脚本`MigrationContext.ps1`。

```pwsh
cd src\Database\EntityFramework.Migrator
.\MigrationContext.ps1
```

## 运行项目
```pwsh
cd src\Http.API
dotnet watch run 
```

# 使用
请查阅[使用文档](https://github.com/AterDev/ater.docs/tree/dev/cn/ater.web.template)！