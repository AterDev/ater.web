using Application.IManager;
using Entity.SystemEntities;
using Ater.Web.Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemLogsDtos;

namespace Application.Test.Managers;

public class SystemLogsManagerTest : BaseTest
{
    private readonly ISystemLogsManager manager;
    public string RandomString { get; set; }

    public SystemLogsManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemLogsManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }
    [Fact]
    public async Task SystemLogs_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var dto = new SystemLogsAddDto()
        {
            ActionUserName = "ActionUserName" + RandomString,
            TargetName = "TargetName" + RandomString,
            Route = "Route" + RandomString,
            ActionType = 0,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.ActionUserName, res.ActionUserName);
        Assert.Equal(entity.TargetName, res.TargetName);
        Assert.Equal(entity.Route, res.Route);
        Assert.Equal(entity.ActionType, res.ActionType);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new SystemLogsUpdateDto()
        {
            ActionUserName = "UpdateActionUserName" + RandomString,
            TargetName = "UpdateTargetName" + RandomString,
            Route = "UpdateRoute" + RandomString,
            ActionType = 0,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.ActionUserName, res.ActionUserName);
            Assert.Equal(entity.TargetName, res.TargetName);
            Assert.Equal(entity.Route, res.Route);
            Assert.Equal(entity.ActionType, res.ActionType);

        }
    }

    async internal Task Should_QueryAsync()
    {
        var filter = new SystemLogsFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
