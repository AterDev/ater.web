using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Share.Models.SystemUserDtos;

namespace Application.Test.Managers;

public class SystemUserManagerTest : BaseTest
{
    private readonly ISystemUserManager manager;

    public SystemUserManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemUserManager>();
    }


    [Fact]
    public async Task Shoud_AddAsync()
    {
        var entity = new SystemUser
        {
            UserName = "Test",
            PasswordSalt = HashCrypto.BuildSalt()
        };
        entity.PasswordHash = HashCrypto.GeneratePwd("123456", entity.PasswordSalt);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.UserName, res.UserName);
    }


    [Fact]
    public async Task Should_UpdateAsync()
    {
        var dto = new SystemUserUpdateDto();
        var entity = manager.Command.Db.FirstOrDefault();

        if (entity != null)
        {
            dto.UserName = "updateUser";
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(dto.UserName, res.UserName);
        }
    }

    [Fact]
    public async Task Should_QueryAsync()
    {
        var filter = new SystemUserFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
