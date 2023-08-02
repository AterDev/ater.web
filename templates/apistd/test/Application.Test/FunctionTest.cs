using Application.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Application.Test;
/// <summary>
/// 方法测试
/// </summary>
public class FunctionTest : BaseTest
{
    private readonly IConfiguration configuration;

    private readonly IEmailService emailService;
    public FunctionTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        this.configuration = Services.GetRequiredService<IConfiguration>();
        emailService = Services.GetRequiredService<IEmailService>();
    }

    /// <summary>
    /// SMTP邮件发送
    /// </summary>
    [Fact]
    public async Task Should_Send_EmailAsync()
    {
        await emailService.SendAsync("zpty@outlook.com", "hello, just test!", "this is content test!");

        Assert.True(true);
    }
}
