using Application.IManager;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemRoleDtos;

namespace Application.Test.Managers;

public class SystemRoleManagerTest : BaseTest
{
    private readonly ISystemRoleManager manager;
    public string RandomString { get; set; }

    public SystemRoleManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemRoleManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }


    [Fact]
    public async Task SystemRole_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }
    async internal Task Shoud_AddAsync()
    {
        var dto = new SystemRoleAddDto()
        {
            Name = "Name" + RandomString,
            NameValue = "NameValue" + RandomString,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Name, res.Name);
        Assert.Equal(entity.NameValue, res.NameValue);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new SystemRoleUpdateDto()
        {
            Name = "Name" + RandomString,
            NameValue = "NameValue" + RandomString,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Name, res.Name);
            Assert.Equal(entity.NameValue, res.NameValue);

        }
    }
    async internal Task Should_QueryAsync()
    {
        var filter = new SystemRoleFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
