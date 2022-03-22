using System.Security.Cryptography;

namespace Core.Utils;

/// <summary>
/// hash加密
/// </summary>
public class HashCrypto
{
    private readonly static RandomNumberGenerator Rng = RandomNumberGenerator.Create();
    public static string Create(string value, string salt)
    {
        var encrpty = new Rfc2898DeriveBytes(value, Encoding.UTF8.GetBytes(salt), 100, HashAlgorithmName.SHA512);
        var valueBytes = encrpty.GetBytes(32);
        return Convert.ToBase64String(valueBytes);
    }
    public static bool Validate(string value, string salt, string hash) => Create(value, salt) == hash;
    public static string BuildSalt()
    {
        var randomBytes = new byte[128 / 8];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
    /// <summary>
    /// 字符串md5值
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Md5Hash(string str)
    {
        using var md5 = MD5.Create();
        var data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
        var sBuilder = new StringBuilder();
        for (var i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }
    /// <summary>
    /// 某文件的md5值
    /// </summary>
    /// <param name="stream">file stream</param>
    /// <returns></returns>
    public static string Md5FileHash(Stream stream)
    {
        using var md5 = MD5.Create();
        var data = md5.ComputeHash(stream);
        var sBuilder = new StringBuilder();
        for (var i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }
    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <param name="length"></param>
    /// <param name="useNum"></param>
    /// <param name="useLow"></param>
    /// <param name="useUpp"></param>
    /// <param name="useSpe"></param>
    /// <param name="custom"></param>
    /// <returns></returns>
    public static string? GetRnd(int length = 4, bool useNum = true, bool useLow = false, bool useUpp = true, bool useSpe = false, string custom = "")
    {
        var b = new byte[4];
        string? s = null;
        var str = custom;
        if (useNum) { str += "0123456789"; }
        if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
        if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
        if (useSpe) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

        // 范围
        var range = str.Length - 1;
        for (var i = 0; i < length; i++)
        {
            Rng.GetBytes(b);
            // 随机数
            var rn = BitConverter.ToUInt32(b, 0) / ((double)uint.MaxValue + 1);
            // 位置
            var position = (int)(rn * range);
            s += str.Substring(position, 1);
        }
        return s;
    }
}
