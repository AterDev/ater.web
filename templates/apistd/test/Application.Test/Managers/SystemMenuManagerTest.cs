using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemMenuDtos;

namespace Application.Test.Managers;

public class SystemMenuManagerTest : BaseTest
{
    private readonly ISystemMenuManager manager;
    public string RandomString { get; set; }

    public SystemMenuManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemMenuManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }

    [Fact]
    public async Task Shoud_AddAsync()
    {
        var dto  = new SystemMenuAddDto()
        {
            Name = "Name" + RandomString,
            AccessCode = "AccessCode" + RandomString,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Name, res.Name);
        Assert.Equal(entity.AccessCode, res.AccessCode);

    }

    [Fact]
    public async Task Should_UpdateAsync()
    {
        var dto  = new SystemMenuUpdateDto()
        {
            Name = "Name" + RandomString,
            AccessCode = "AccessCode" + RandomString,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Name, res.Name);
            Assert.Equal(entity.AccessCode, res.AccessCode);

        }
    }

    [Fact]
    public async Task Should_QueryAsync()
    {
        var filter = new SystemMenuFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
