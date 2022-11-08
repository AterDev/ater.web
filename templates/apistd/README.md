# 说明
`ater.web.template` 项目模板的使用提供文档支持。

## 项目结构说明
- scripts 脚本文件目录
- src 代码目录
  - Microservice 拆分可单独部署的应用，如微服务项目。
  - Core 基础核心类库，包括基础库的扩展，辅助类以及实体模型等.
  - Share 共享类库，可在多个应用间共用的内容，如各类模型定义、通用配置、数据处理转换等，依赖 `Core`.
  - Application 应用功能类库，包括但不限于 仓储服务、消息服务、缓存服务等。依赖 `Share`.
  - Http.API API接口项目，定义和编写控制器，对外开放和运行的主程序，依赖`Application`
  - Database 数据库目录，如EntityFramework数据库上下文及迁移项目.
    - EntityFramework 管理和配置`DbContext`.
    - EntityFramework.Migrator 单独的数据库迁移项目。
  - WebApp 浏览器Web应用目录

- test 测试项目目录

# 规范及约定

## EF模型定义
遵循`Entity Framework Core`的官方文档，对模型及关联关系进行定义。

通过`dotnet ef `命令进行数据库结构的迁移。

## CQRS仓储模式
模板提供了两个默认实现的仓储基类`CommandStoreBase`和`QueryStoreBase`，在`Application`项目的`Implement`目录中.分别代表命令(可读写)仓储和查询仓储(只读仓储)

使用`droplet`工具会根据您的实体模型，自动生成对应的仓储，分别继承`CommandStaoreBase`和`QueryStoreBase`。在生成的仓储类中，你可以自由实现自己的数据库操作方法。

>使用`droplet`生成会自动注入仓储服务，无需手动注入。
## 业务接口模式
通过接口定义业务方法，模板中提供`IDomainManager`接口，提供`MomainManagerBase`类作为默认实现。

通过`droplet`芽会根据您的实体模型，自动生成对应的业务接口和实现类。在生成的业务实现类中，你可以自由实现自己的业务逻辑。

>使用`droplet`生成会自动注入业务接口服务，无需手动注入。

## 接口请求与返回
整体以RESTful风格为标准，进行一定的简化。

### 请求方式
- GET，获取数据时使用GET，复杂的筛选和条件查询，可改用POST方式传递参数。
- POST，添加数据时使用POST。主体参数使用JSON格式。
- PUT，修改数据时使用PUT。主体参数使用JSON格式。
- DELETE，删除数据时使用DELETE。

### 请求返回
返回以HTTP状态码为准。
- 200，执行成功。
- 201，创建成功。
- 401，未验证，没有传递token或token已失效。需要重新获取token(登录)。
- 403，禁止访问，指已登录的用户但没有权限访问。
- 404，请求的资源不存在。
- 409，资源冲突。
- 500，错误返回，服务器出错或业务错误封装。

接口请求成功时， 前端可直接获取数据。

接口请求失败时，返回统一的错误格式。

前端根据HTTP状态码判断请求是否成功，然后获取数据。

错误返回的格式如下：
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "",
  "status": 500,
  "detail": "未知的错误！",
  "traceId": "00-d768e1472decd92538cdf0a2120c6a31-a9d7310446ea4a3f-00"
}
```
### ASP.NET Core 请求返回示例
1. 路由定义，约定使用HTTP谓词，不使用Route。
请参见 [**HTTP谓词模板**](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/routing?view=aspnetcore-6.0#http-verb-templates)。
2. **模型绑定**，可使用`[Frombody]`以及`[FromRoute]`指明请求来源，
参见[**请求来源**](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/models/model-binding?view=aspnetcore-6.0#sources)，如：
```csharp
// 修改信息
[HttpPut("{id}")]
public async Task<ActionResult<TEntity?>> UpdateAsync([FromRoute] Guid id, TUpdate form)
```

1. 关于返回类型，请使用[ActionResult&#60;T&#62;或特定类型](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0#actionresult-vs-iactionresult)作为返回类型。
- 正常返回，可直接返回特定类型数据。
- 错误返回,使用Problem()，如：
```csharp
// 如果错误，使用Problem返回内容
return Problem("未知的错误！", title: "业务错误");
```
- 404，使用NotFound()，如：
```csharp
// 如果不存在，返回404
return NotFound("用户名密码不存在");
```