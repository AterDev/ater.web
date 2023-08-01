using Application.IManager;
using Entity.SystemEntities;
using Ater.Web.Core.Utils;
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
    public async Task SystemMenu_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var dto = new SystemMenuAddDto()
        {
            Name = "Name" + RandomString,
            IsValid = true,
            AccessCode = "AccessCode" + RandomString,
            MenuType = 0,
            Sort = 0,
            Hidden = true,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Name, res.Name);
        Assert.Equal(entity.IsValid, res.IsValid);
        Assert.Equal(entity.AccessCode, res.AccessCode);
        Assert.Equal(entity.MenuType, res.MenuType);
        Assert.Equal(entity.Sort, res.Sort);
        Assert.Equal(entity.Hidden, res.Hidden);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new SystemMenuUpdateDto()
        {
            Name = "UpdateName" + RandomString,
            IsValid = true,
            AccessCode = "UpdateAccessCode" + RandomString,
            MenuType = 0,
            Sort = 0,
            Hidden = true,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Name, res.Name);
            Assert.Equal(entity.IsValid, res.IsValid);
            Assert.Equal(entity.AccessCode, res.AccessCode);
            Assert.Equal(entity.MenuType, res.MenuType);
            Assert.Equal(entity.Sort, res.Sort);
            Assert.Equal(entity.Hidden, res.Hidden);

        }
    }

    async internal Task Should_QueryAsync()
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
