using CMS.IManager;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Share.Models.BlogDtos;

namespace Application.Test.Managers;

public class BlogManagerTest : BaseTest
{
    private readonly IBlogManager manager;
    public string RandomString { get; set; }

    public BlogManagerTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        manager = Services.GetRequiredService<IBlogManager>();
        RandomString = DateTime.Now.ToString("MMddmmss");
    }
    [Fact]
    public async Task Blog_Should_Pass()
    {
        await Shoud_AddAsync();
        await Should_UpdateAsync();
        await Should_QueryAsync();
    }

    async internal Task Shoud_AddAsync()
    {
        var dto = new BlogAddDto()
        {
            Title = "Title" + RandomString,
            Content = "Content" + RandomString,
            Authors = "Authors" + RandomString,
            IsAudit = true,
            IsPublic = true,
            IsOriginal = true,
            UserId = new Guid(""),
            CatalogId = new Guid(""),
            ViewCount = 0,
        };
        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        Assert.Equal(entity.Title, res.Title);
        Assert.Equal(entity.Content, res.Content);
        Assert.Equal(entity.Authors, res.Authors);
        Assert.Equal(entity.IsAudit, res.IsAudit);
        Assert.Equal(entity.IsPublic, res.IsPublic);
        Assert.Equal(entity.IsOriginal, res.IsOriginal);
        Assert.Equal(entity.ViewCount, res.ViewCount);

    }

    async internal Task Should_UpdateAsync()
    {
        var dto = new BlogUpdateDto()
        {
            Title = "UpdateTitle" + RandomString,
            Content = "UpdateContent" + RandomString,
            Authors = "UpdateAuthors" + RandomString,
            IsAudit = true,
            IsPublic = true,
            IsOriginal = true,
            UserId = new Guid(""),
            CatalogId = new Guid(""),
            ViewCount = 0,
        };
        var entity = await manager.Command.Db.FirstOrDefaultAsync();
        if (entity != null)
        {
            var res = await manager.UpdateAsync(entity, dto);
            Assert.Equal(entity.Title, res.Title);
            Assert.Equal(entity.Content, res.Content);
            Assert.Equal(entity.Authors, res.Authors);
            Assert.Equal(entity.IsAudit, res.IsAudit);
            Assert.Equal(entity.IsPublic, res.IsPublic);
            Assert.Equal(entity.IsOriginal, res.IsOriginal);
            Assert.Equal(entity.ViewCount, res.ViewCount);

        }
    }

    async internal Task Should_QueryAsync()
    {
        var filter = new BlogFilterDto()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var res = await manager.FilterAsync(filter);
        Assert.True(res != null && res.Count != 0 && res.Data.Count <= 2);
    }
}
