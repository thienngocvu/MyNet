using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MyNet.Application.Common.Extensions
{
    public static class StringExtension
    {
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("9awKZOXZDgzg9sH4Fzfgd4ux3RqYO43D");
        private static readonly byte[] _iv = Encoding.UTF8.GetBytes("Ez4s3FwaqlxfPm4h");

        public static T ConvertTo<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                    return (T)converter.ConvertFromString(input);
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static int ConvertToInt(this string input)
        {
            try
            {
                int value;
                int.TryParse(input, out value);
                return value;
            }
            catch (NotSupportedException)
            {
                return 0;
            }
        }

        public static bool EqualIgnoreCase(this string source, string toCheck)
        {
            return string.Equals(source, toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static string ByteToString(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException("input");
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", "b");
        }

        public static string EncryptString(string plainText)
        {
            byte[] array;

            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return HttpUtility.UrlEncode(Convert.ToBase64String(array));
        }

        public static string DecryptString(string cipherText)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string StringToMd5(this string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
