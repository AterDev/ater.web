using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Share.Models.SystemConfigDtos;

namespace Application.Test.Managers;

public class SystemConfigManagerTest : BaseTest
{
    private readonly ISystemConfigManager manager;
    public string RandomString { get; set; }

    public SystemConfigManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemConfigManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }

    [Fact]
    public async Task Shoud_AddAsync()
    {
        // var entity = new SystemConfig(){ Name = "" + RandomString};
        // var res = await manager.AddAsync(entity);
        // Assert.Equal(entity.UserName, res.UserName);
    }


    [Fact]
    public async Task Should_UpdateAsync()
    {
        var dto = new SystemConfigUpdateDto();
        var entity = manager.Command.Db.FirstOrDefault();

        if (entity != null)
        {
            // dto.UserName = "updateUser" + RandomString;
            var res = await manager.UpdateAsync(entity, dto);
            // Assert.Equal(dto.UserName, res.UserName);
        }
    }

    [Fact]
    public async Task Should_QueryAsync()
    {
        var filter = new SystemConfigFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
