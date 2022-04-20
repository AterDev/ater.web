using SendGrid;
using SendGrid.Helpers.Mail;
using Share.Options;

namespace Share;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string fromEmail = "no-response@your.com";
    private readonly string _webName = "DevPlf";
    private readonly IHostEnvironment _env;
    public EmailService(IOptions<MailOption> options, IHostEnvironment env)
    {
        _apiKey = options.Value.APIKey!;
        _env = env;
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="fromEamil"></param>
    /// <param name="toEmail"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <param name="htmlContent"></param>
    /// <param name="fromName"></param>
    /// <param name="toName"></param>
    /// <returns></returns>
    public async Task<Response> SendAsync(string fromEamil, string toEmail, string subject, string? content = null, string? htmlContent = null, string? fromName = null, string? toName = null)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(fromEamil, fromName);
        var to = new EmailAddress(toEmail, toName);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, htmlContent);
        var response = await client.SendEmailAsync(msg);
        return response;

    }

    /// <summary>
    /// 注册激活邮件
    /// </summary>
    /// <param name="toEmail"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> SendRegisterMailAsync(string toEmail, string userId)
    {
        var host = "yourhost";
        if (_env.IsDevelopment())
        {
            host = "http://localhost:4200";
        }
        var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var htmlContent = $"<p>如果您没有注册【{_webName}】账户，请忽略该邮件</p>";
        htmlContent += "<p>请点击以下链接激活验证您的邮箱（两小时内有效）：</p>";
        htmlContent += $@"<a href=""{host}/home/verify_email?code={userId}&time={time}""
style=""background: #1e5b99;color:rgb(255, 255, 255);padding:8px 16px;text-decoration: none"">激活邮箱</a>";
        var response = await SendAsync(fromEmail, toEmail, $"{_webName}账号激活", null, htmlContent, _webName);
        if (response.StatusCode != HttpStatusCode.OK
            && response.StatusCode != HttpStatusCode.Accepted)
        {
            // TODO:记录错误信息
            var result = await response.Body.ReadAsStringAsync();
            Console.WriteLine(result);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="toEmail"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<bool> SendVerifyCodeAsync(string toEmail, string code)
    {
        var htmlContent = @$"<p>您请求的【{_webName}】验证码为：</p>";
        htmlContent += $@"<p style=""color:blue;font-size:20px""><strong>{code}</strong></p>";
        var response = await SendAsync(fromEmail, toEmail, $"{_webName}验证码", null, htmlContent, _webName);
        if (response.StatusCode != HttpStatusCode.OK
            && response.StatusCode != HttpStatusCode.Accepted)
        {
            // TODO:记录错误信息
            var result = await response.Body.ReadAsStringAsync();
            Console.WriteLine(result);
            return false;
        }
        return true;
    }
}
