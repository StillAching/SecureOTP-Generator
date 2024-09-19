using System.Security.Cryptography;

namespace SecureOTPGenerator
{
    public class OTPGenerator
    {
        public static string GenerateOTP(string base32Key, long counter, int digits, HashAlgorithm algo)
        {
            if (string.IsNullOrWhiteSpace(base32Key))
                throw new ArgumentException("Base32 key cannot be null or empty.");

            if (digits <= 0)
                throw new ArgumentException("Digits must be a positive integer.");


            byte[] bytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            byte[] key = Base32Decode(base32Key);
            byte[] hash;
            switch (algo)
            {
                case HashAlgorithm.MD5:
                    hash = new HMACMD5(key).ComputeHash(bytes);
                    break;

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
                    throw new ArgumentException("Invalid algorithm. Please use any one of SHA1, SHA256, SHA512 or MD5");
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
            if (string.IsNullOrEmpty(base32EncodedString))
                throw new ArgumentNullException(nameof(base32EncodedString));

            const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            base32EncodedString = base32EncodedString.TrimEnd('=').ToUpperInvariant();

            int byteCount = base32EncodedString.Length * 5 / 8;
            byte[] result = new byte[byteCount];

            int buffer = 0;
            int bitsLeft = 0;
            int index = 0;

            foreach (char c in base32EncodedString)
            {
                int value = Base32Alphabet.IndexOf(c);
                if (value < 0)
                    throw new FormatException($"Invalid Base32 character '{c}' in input.");

                buffer <<= 5;
                buffer |= value & 31;
                bitsLeft += 5;

                if (bitsLeft >= 8)
                {
                    result[index++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return result;
        }

    }
}
