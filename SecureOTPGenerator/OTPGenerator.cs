using System.Security.Cryptography;

namespace SecureOTPGenerator
{
    public enum HashAlgorithm
    {
        SHA1,
        SHA256,
        SHA512
    }

    public class OTPGenerator
    {
        public static string GenerateOTP(string base32Key, long counter, int digits, HashAlgorithm algo)
        {
            byte[] bytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            byte[] key = Base32Decode(base32Key);
            byte[] hash;
            switch (algo)
            {
                case HashAlgorithm.SHA1:
                    hash = new HMACSHA1(key).ComputeHash(bytes);
                    break;

                case HashAlgorithm.SHA256:
                    hash = new HMACSHA256(key).ComputeHash(bytes);
                    break;

                case HashAlgorithm.SHA512:
                    hash = new HMACSHA512(key).ComputeHash(bytes);
                    break;

                default:
                    throw new ArgumentException("Invalid algorithm. Please use any one of SHA1, SHA256, or SHA512");
            }

            int sourceIndex = hash[hash.Length - 1] & 0xF;
            byte[] truncatedHash = new byte[4];
            Array.Copy(hash, sourceIndex, truncatedHash, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(truncatedHash);
            }

            uint value = BitConverter.ToUInt32(truncatedHash, 0) & 0x7FFFFFFFu;
            string otp = (value % (uint)Math.Pow(10.0, digits)).ToString().PadLeft(digits, '0');
            return otp;
        }

        private static byte[] Base32Decode(string base32EncodedString)
        {
            const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            base32EncodedString = base32EncodedString.TrimEnd('=');
            string bits = string.Join("", from c in base32EncodedString.ToUpper() select Convert.ToString(Base32Alphabet.IndexOf(c), 2).PadLeft(5, '0'));
            byte[] bytes = (from i in Enumerable.Range(0, bits.Length / 8) select Convert.ToByte(bits.Substring(i * 8, 8), 2)).ToArray();
            return bytes.Take(bytes.Length - bytes.Length % 5 / 5).ToArray();
        }
    }
}
