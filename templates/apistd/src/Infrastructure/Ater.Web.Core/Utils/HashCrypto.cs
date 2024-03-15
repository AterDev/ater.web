
global using System.Text.Json;
global using System.Text.Json.Serialization;

namespace Ater.Web.Core.Utils;
/// <summary>
/// 提供常用加解密方法
/// </summary>
public class HashCrypto
{
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    /// <summary>
    /// SHA512 encrypt
    /// </summary>
    /// <param name="value"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public static string GeneratePwd(string value, string salt)
    {
        Rfc2898DeriveBytes encrpty = new(value, Encoding.UTF8.GetBytes(salt), 100, HashAlgorithmName.SHA512);
        var valueBytes = encrpty.GetBytes(32);
        return Convert.ToBase64String(valueBytes);
    }

    public static bool Validate(string value, string salt, string hash)
    {
        return GeneratePwd(value, salt) == hash;
    }

    public static string BuildSalt()
    {
        var randomBytes = new byte[128 / 8];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// HMACSHA256 encrypt
    /// </summary>
    /// <param name="key"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string HMACSHA256(string key, string content)
    {
        using HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(key));
        var valueBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(valueBytes);

    }

    /// <summary>
    /// 字符串md5值
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Md5Hash(string str)
    {
        var data = MD5.HashData(Encoding.UTF8.GetBytes(str));
        StringBuilder sBuilder = new();
        for (var i = 0; i < data.Length; i++)
        {
            _ = sBuilder.Append(data[i].ToString("x2"));
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
        StringBuilder sBuilder = new();
        for (var i = 0; i < data.Length; i++)
        {
            _ = sBuilder.Append(data[i].ToString("x2"));
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
    public static string GetRnd(int length = 4, bool useNum = true, bool useLow = false, bool useUpp = true, bool useSpe = false, string custom = "")
    {
        var b = new byte[4];
        string s = string.Empty;
        var str = custom;
        if (useNum)
        {
            str += "0123456789";
        }
        if (useLow)
        {
            str += "abcdefghijklmnopqrstuvwxyz";
        }
        if (useUpp)
        {
            str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }
        if (useSpe)
        {
            str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
        }

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

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="text">源文</param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string AesEncrypt(string text, string key)
    {
        byte[] encrypted;
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Md5Hash(key));
            aesAlg.IV = aesAlg.Key[..16];
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            using MemoryStream memoryStream = new();
            using var csEncrypt = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            csEncrypt.Write(bytes, 0, bytes.Length);
            csEncrypt.FlushFinalBlock();
            encrypted = memoryStream.ToArray();
        }
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string AesDecrypt(string cipherText, string key)
    {
        if (string.IsNullOrWhiteSpace(cipherText))
        {
            return string.Empty;
        }
        string? plaintext = null;
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Md5Hash(key));
            aesAlg.IV = aesAlg.Key[..16];
            ICryptoTransform decryptor = aesAlg.CreateDecryptor();
            using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt, Encoding.UTF8);
            plaintext = srDecrypt.ReadToEnd();
        }
        return plaintext;
    }

    /// <summary>
    /// json对象加密
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string JsonEncrypt(object data)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(data, JsonSerializerOptions);

        if (bytes != null)
        {
            bytes = bytes.Select(b => b == byte.MaxValue ? byte.MinValue : (byte)(b + 1))
                .ToArray();
            bytes = bytes.Reverse().ToArray();
            return Convert.ToBase64String(bytes);
        }
        return string.Empty;
    }

    /// <summary>
    /// json对象解密
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? JsonDecrypt<T>(string value) where T : class
    {
        var bytes = Convert.FromBase64String(value);
        if (bytes != null)
        {
            bytes = bytes.Reverse().ToArray();
            bytes = bytes.Select(b => b == byte.MinValue ? byte.MaxValue : (byte)(b - 1))
                .ToArray();
            var jsonString = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<T>(jsonString, JsonSerializerOptions)!;
        }
        return null;
    }
}
