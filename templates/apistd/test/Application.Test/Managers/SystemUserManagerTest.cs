using Application.IManager;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.SystemUserDtos;

namespace Application.Test.Managers;

public class SystemUserManagerTest : BaseTest
{
    private readonly ISystemUserManager manager;
    public string RandomString { get; set; }

    public SystemUserManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<ISystemUserManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }


    [Fact]
    public async Task SystemUser_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var salt = HashCrypto.BuildSalt();
        var dto = new SystemUserAddDto()
        {
            UserName = "UserName" + RandomString,
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd("PasswordHash" + RandomString, salt)
        };

        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.UserName, res.UserName);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new SystemUserUpdateDto()
        {
            UserName = "UserName" + RandomString,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.UserName, res.UserName);

        }
    }

    async internal Task Should_QueryAsync()
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
