using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// 密码哈希加盐工具类
    /// 用于安全的密码存储和验证
    /// </summary>
    public static class PasswordHasher
    {
        // 盐值长度
        private const int SaltSize = 32;

        // 哈希长度
        private const int HashSize = 32;

        // 迭代次数
        private const int Iterations = 10000;

        /// <summary>
        /// 生成随机盐值
        /// </summary>
        /// <returns>Base64编码的盐值</returns>
        public static string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[SaltSize];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        /// <summary>
        /// 使用PBKDF2算法生成密码哈希
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="salt">盐值</param>
        /// <returns>Base64编码的哈希值</returns>
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                byte[] hashBytes = pbkdf2.GetBytes(HashSize);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="password">用户输入的密码</param>
        /// <param name="storedHash">数据库中存储的哈希值</param>
        /// <param name="storedSalt">数据库中存储的盐值</param>
        /// <returns>是否验证通过</returns>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                // 计算输入密码的哈希
                string computedHash = HashPassword(password, storedSalt);

                // 使用恒定时间比较，防止时序攻击
                return SlowEquals(Convert.FromBase64String(computedHash), Convert.FromBase64String(storedHash));
            }
            catch (FormatException)
            {
                // 如果数据格式错误，直接返回false
                return false;
            }
            catch (Exception)
            {
                // 其他异常也返回false
                return false;
            }
        }

        /// <summary>
        /// 恒定时间比较两个字节数组
        /// 防止时序攻击
        /// </summary>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// 一次性生成盐值和哈希
        /// 用于新用户注册
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <returns>包含盐值和哈希值的元组</returns>
        public static (string salt, string hash) CreateHash(string password)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);
            return (salt, hash);
        }

        /// <summary>
        /// 简单密码强度检查
        /// </summary>
        public static bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;

            // 检查是否包含数字
            bool hasDigit = false;
            // 检查是否包含字母
            bool hasLetter = false;

            foreach (char c in password)
            {
                if (char.IsDigit(c)) hasDigit = true;
                if (char.IsLetter(c)) hasLetter = true;

                if (hasDigit && hasLetter) break;
            }

            return hasDigit && hasLetter;
        }
    }
}