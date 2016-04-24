using System;
using System.Security.Cryptography;
using System.Text;

//! \brief Tools to manage secondary business handled through third party libraries.
//!
//! These libraries are basically : an ORM (EntityFramework), Authentication (JsonWT), Hash (MD5Hash).
namespace ConceptionDevisWS.Services.Utils
{
    /// <summary>
    /// Handle Hashes
    /// </summary>
    public static class HashManager
    {
        /// <summary>
        /// Get an SHA256 hash of the given string.
        /// </summary>
        /// <param name="data">the data to hash</param>
        /// <returns>the hash</returns>
        public static string GetHash(string data)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            string hashedData = string.Join("", Array.ConvertAll<byte,string>(hashedBytes, b=> string.Format("{0:X2}", b)));
            return hashedData;
        }
    }
}