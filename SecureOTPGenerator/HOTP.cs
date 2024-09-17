
namespace SecureOTPGenerator
{
    public class HOTP
    {
        public string Secret { get; set; }
        public int Digits { get; set; } = 6;
        public long Counter { get; set; } = 0L;

        public string Generate()
        {
            if (string.IsNullOrEmpty(Secret))
            {
                throw new ArgumentException("No secret key provided!!!!");
            }

            if (Digits <= 0)
            {
                Digits = 6;
            }

            return OTPGenerator.GenerateOTP(Secret, Counter, Digits, HashAlgorithm.SHA1);
        }
    }
}
