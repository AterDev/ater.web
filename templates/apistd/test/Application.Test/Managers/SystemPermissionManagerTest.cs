using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemPermissionDtos;

namespace Application.Test.Managers;

public class SystemPermissionManagerTest : BaseTest
{
    private readonly ISystemPermissionManager manager;
    public string RandomString { get; set; }

    public SystemPermissionManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemPermissionManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }

    [Fact]
    public async Task Shoud_AddAsync()
    {
        var dto  = new SystemPermissionAddDto()
        {
            Name = "Name" + RandomString,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Name, res.Name);

    }

    [Fact]
    public async Task Should_UpdateAsync()
    {
        var dto  = new SystemPermissionUpdateDto()
        {
            Name = "Name" + RandomString,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Name, res.Name);

        }
    }

    [Fact]
    public async Task Should_QueryAsync()
    {
        var filter = new SystemPermissionFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
