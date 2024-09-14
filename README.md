# What is SecureOTP-Generator?

**SecureOTP-Generator** is a C# library for generating Time-based One-Time Passwords (TOTP) and HMAC-based One-Time Passwords (HOTP), in accordance with the standards specified in [RFC 4226](https://tools.ietf.org/html/rfc4226) and [RFC 6238](https://tools.ietf.org/html/rfc6238). These OTPs are commonly used in two-factor authentication (2FA) systems to enhance security.

## Features

- **TOTP Generation**: Generate time-based OTPs with configurable time periods, algorithms, and digits.
- **HOTP Generation**: Generate counter-based OTPs with configurable counters, algorithms, and digits.
- **Configurable Algorithms**: Supports SHA1, SHA256, and SHA512 hashing algorithms.
- **Base32 Secret Keys**: Utilizes Base32-encoded secret keys for secure OTP generation.

## Installation
To integrate this library into your project:

Download the lib from the latest releases: https://github.com/StillAching/SecureOTP-Generator/releases
Then reference the file in your project.

OR

1. Clone the repository:
    ```sh
    git clone https://github.com/StillAching/SecureOTP-Generator.git
    ```

2. Add the `.cs` files from the cloned repository to your C# project.

## Usage & Examples

### TOTP Example

To generate a TOTP, instantiate the `TOTP` class and configure it as follows:

```csharp
using System;

public class Program
{
    public static void Main()
    {
        var totp = new TOTP
        {
            Secret = "PUT SECRET HERE!!!", // Required
            Digits = 6,                    // Optional (default is 6)
            Algorithm = "SHA1",            // Optional (default is SHA1)
            Period = 30,                   // Optional (default is 30 seconds)
            UnixTime = 99999999999         // Optional (default is current Unix time)
        };

        string otp = totp.Generate();
        Console.WriteLine($"Generated TOTP: {otp}");
    }
}
```

### HOTP Example

To generate an HOTP, use the `HOTP` class:

```csharp
using System;

public class Program
{
    public static void Main()
    {
        var hotp = new HOTP
        {
            Secret = "PUT SECRET HERE!!!", // Required
            Digits = 6,                    // Optional (default is 6)
            Counter = 0                    // Optional (default is 0)
        };

        string otp = hotp.Generate();
        Console.WriteLine($"Generated HOTP: {otp}");
    }
}
```

## Configuration

### TOTP Configuration

- **Secret**: Base32-encoded string used as the secret key. (Required)
- **Digits**: Number of digits in the OTP (default is 6).
- **Algorithm**: Hashing algorithm to use (`SHA1`, `SHA256`, `SHA512`). Default is `SHA1`.
- **Period**: Time period in seconds for the TOTP (default is 30 seconds).
- **UnixTime**: Unix timestamp for generating the OTP. Defaults to the current time.

### HOTP Configuration

- **Secret**: Base32-encoded string used as the secret key. (Required)
- **Digits**: Number of digits in the OTP (default is 6).
- **Counter**: Counter value used for generating the OTP (default is 0).

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
