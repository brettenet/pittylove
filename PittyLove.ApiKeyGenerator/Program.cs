using System;
using System.Security.Cryptography;

namespace PittyLove.ApiKeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var privateKey = new byte[64];
                var publicKey = new byte[64];
                provider.GetBytes(publicKey);
                provider.GetBytes(privateKey);
                Console.WriteLine("Public: {0}\nPrivate: {1}", Convert.ToBase64String(publicKey), Convert.ToBase64String(privateKey));
            }
            Console.ReadLine();
        }
    }
}
