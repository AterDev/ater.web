using Application.IManager;
using Core.Entities.SystemEntities;
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
        var entity = new SystemUser() { UserName = "Test" };
        var res = await manager.AddAsync(entity);
        Assert.NotNull(res);
    }
}
