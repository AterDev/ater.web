using Application.IManager;
using Entity.SystemEntities;
using Ater.Web.Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemPermissionGroupDtos;

namespace Application.Test.Managers;

public class SystemPermissionGroupManagerTest : BaseTest
{
    private readonly ISystemPermissionGroupManager manager;
    public string RandomString { get; set; }

    public SystemPermissionGroupManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemPermissionGroupManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }
    [Fact]
    public async Task SystemPermissionGroup_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var dto = new SystemPermissionGroupAddDto()
        {
            Name = "Name" + RandomString,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Name, res.Name);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new SystemPermissionGroupUpdateDto()
        {
            Name = "UpdateName" + RandomString,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Name, res.Name);

        }
    }

    async internal Task Should_QueryAsync()
    {
        var filter = new SystemPermissionGroupFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
