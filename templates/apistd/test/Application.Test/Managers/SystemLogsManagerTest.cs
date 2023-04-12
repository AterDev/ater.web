using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
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
    public async Task Shoud_AddAsync()
    {
        var dto  = new SystemLogsAddDto()
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

    [Fact]
    public async Task Should_UpdateAsync()
    {
        var dto  = new SystemLogsUpdateDto()
        {
            ActionUserName = "ActionUserName" + RandomString,
            TargetName = "TargetName" + RandomString,
            Route = "Route" + RandomString,
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

    [Fact]
    public async Task Should_QueryAsync()
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
