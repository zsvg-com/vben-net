using System.Security.Cryptography;

namespace Vben.Base.Sys.User;

public class AesHelper
{
    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="encryptedData">加密后的Base64字符串</param>
    /// <param name="key">密钥</param>
    /// <param name="iv">初始化向量</param>
    /// <returns>解密后的明文</returns>
    public static string Decrypt(string encryptedData, string key, string iv)
    {
        try
        {
            // 将Base64字符串转换为字节数组
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            
            // 创建AES实例
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // 创建解密器
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"AES解密失败: {ex.Message}");
        }
    }

    /// <summary>
    /// AES加密（用于测试验证）
    /// </summary>
    public static string Encrypt(string data, string key, string iv)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(dataBytes, 0, dataBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}