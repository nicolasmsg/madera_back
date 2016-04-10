using System;
using System.Security.Cryptography;
using System.Text;

namespace ConceptionDevisWS.Services.Utils
{
    public static class HashManager
    {
        public static string GetHash(string data)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            string hashedData = string.Join("", Array.ConvertAll<byte,string>(hashedBytes, b=> string.Format("{0:X2}", b)));
            return hashedData;
        }
    }
}