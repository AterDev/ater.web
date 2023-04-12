using Application.IManager;
using Core.Entities;
using Core.Utils;
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
    public async Task Shoud_AddAsync()
    {
        var salt = HashCrypto.BuildSalt();

        var dto  = new UserAddDto()
        {
            UserName = "UserName" + RandomString,
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd("PasswordHash" + RandomString, salt)
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.UserName, res.UserName);

    }

    [Fact]
    public async Task Should_UpdateAsync()
    {
        var dto  = new UserUpdateDto()
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

    [Fact]
    public async Task Should_QueryAsync()
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
