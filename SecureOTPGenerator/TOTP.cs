
namespace SecureOTPGenerator
{
    public class TOTP
    {
        public string Secret { get; set; }
        public int Digits { get; set; } = 6;
        public HashAlgorithm Algorithm { get; set; } = HashAlgorithm.SHA1;
        public long Period { get; set; } = 30L;
        public long UnixTime { get; set; }

        public string Generate()
        {
            if (string.IsNullOrEmpty(Secret))
            {
                throw new ArgumentException("No secret key provided");
            }

            if (Digits <= 0)
            {
                Digits = 6;
            }

            if (Period <= 0)
            {
                Period = 30L;
            }

            long unixTime = UnixTime == 0 ? DateTimeOffset.UtcNow.ToUnixTimeSeconds() : UnixTime;

            return OTPGenerator.GenerateOTP(Secret, unixTime / Period, Digits, Algorithm);
        }

    }
}
