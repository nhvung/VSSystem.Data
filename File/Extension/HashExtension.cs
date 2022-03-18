using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace VSSystem.Data.File.Extensions
{
    public static class HashExtension
    {
        public static byte[] GetSha1Hash(this byte[] input)
        {
            var sha1 = SHA1.Create();
            return sha1.ComputeHash(input);
        }
        public static byte[] GetMd5Hash(this byte[] input)
        {
            var md5 = MD5.Create();
            return md5.ComputeHash(input);
        }
    }
}
