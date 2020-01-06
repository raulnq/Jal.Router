using Jal.Router.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Jal.Router.Impl
{
    public class Hasher : IHasher
    {
        private byte[] GetHashBytes(string input)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        public string Hash(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHashBytes(input))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}