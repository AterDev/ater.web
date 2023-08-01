using Microsoft.AspNetCore.Mvc.Testing;

namespace Application.Test;
/// <summary>
/// 方法测试
/// </summary>
public class FunctionTest : BaseTest
{
    private readonly IConfiguration configuration;
    public FunctionTest(WebApplicationFactory<Program> factory) : base(factory)
    {
        this.configuration = Services.GetRequiredService<IConfiguration>();
    }

    /// <summary>
    /// SMTP邮件发送
    /// </summary>
    [Fact]
    public async Task Should_Send_EmailAsync()
    {

        await Console.Out.WriteLineAsync();

    }
}
