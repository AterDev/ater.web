using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemOrganizationDtos;

namespace Application.Test.Managers;

public class SystemOrganizationManagerTest : BaseTest
{
    private readonly ISystemOrganizationManager manager;
    public string RandomString { get; set; }

    public SystemOrganizationManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemOrganizationManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }

    [Fact]
    public async Task Shoud_AddAsync()
    {
        var dto  = new SystemOrganizationAddDto()
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
        var dto  = new SystemOrganizationUpdateDto()
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
        var filter = new SystemOrganizationFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
