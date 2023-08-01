using Application.IManager;
using Entity;
using Ater.Web.Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.UserDtos;

namespace Application.Test.Managers;

public class UserManagerTest : BaseTest
{
    private readonly IUserManager manager;
    public string RandomString { get; set; }

    public UserManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<IUserManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }
    [Fact]
    public async Task User_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var dto = new UserAddDto()
        {
            UserName = "UserName" + RandomString,
            UserType = 0,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = true,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            RetryCount = 0,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.UserName, res.UserName);
        Assert.Equal(entity.UserType, res.UserType);
        Assert.Equal(entity.EmailConfirmed, res.EmailConfirmed);
        Assert.Equal(entity.PhoneNumberConfirmed, res.PhoneNumberConfirmed);
        Assert.Equal(entity.TwoFactorEnabled, res.TwoFactorEnabled);
        Assert.Equal(entity.LockoutEnabled, res.LockoutEnabled);
        Assert.Equal(entity.AccessFailedCount, res.AccessFailedCount);
        Assert.Equal(entity.RetryCount, res.RetryCount);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new UserUpdateDto()
        {
            UserName = "UpdateUserName" + RandomString,
            UserType = 0,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = true,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            RetryCount = 0,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.UserName, res.UserName);
            Assert.Equal(entity.UserType, res.UserType);
            Assert.Equal(entity.EmailConfirmed, res.EmailConfirmed);
            Assert.Equal(entity.PhoneNumberConfirmed, res.PhoneNumberConfirmed);
            Assert.Equal(entity.TwoFactorEnabled, res.TwoFactorEnabled);
            Assert.Equal(entity.LockoutEnabled, res.LockoutEnabled);
            Assert.Equal(entity.AccessFailedCount, res.AccessFailedCount);
            Assert.Equal(entity.RetryCount, res.RetryCount);

        }
    }

    async internal Task Should_QueryAsync()
    {
        var filter = new UserFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
