using Application.IManager;
using Core.Entities.SystemEntities;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;

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
}
