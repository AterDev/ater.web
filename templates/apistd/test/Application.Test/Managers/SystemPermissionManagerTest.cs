using Application.IManager;
using Entity.SystemEntities;
using Ater.Web.Core.Utils;
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
    public async Task SystemPermission_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var dto = new SystemPermissionAddDto()
        {
            Name = "Name" + RandomString,
            Enable = true,
            PermissionType = 0,
            SystemPermissionGroupId = new Guid(""),
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Name, res.Name);
        Assert.Equal(entity.Enable, res.Enable);
        Assert.Equal(entity.PermissionType, res.PermissionType);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new SystemPermissionUpdateDto()
        {
            Name = "UpdateName" + RandomString,
            Enable = true,
            PermissionType = 0,
            SystemPermissionGroupId = new Guid(""),
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Name, res.Name);
            Assert.Equal(entity.Enable, res.Enable);
            Assert.Equal(entity.PermissionType, res.PermissionType);

        }
    }

    async internal Task Should_QueryAsync()
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
