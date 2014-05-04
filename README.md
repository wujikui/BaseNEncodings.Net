BaseNEncodings.Net
==================

BaseNEncodings.Net is a general base16, base32, base64 encodings library for .NET 2.0+, which is according to [RFC 4648][1].

> The project [java-base-n-encodings][9] was migrated from BaseNEncodings.Net.

Features
--------
- Represents a **general** Base-N data encoding as an abstract class.
- Implements **standard** encodings of **RFC 4648**.
    - Base 64 Encoding
    - Base 64 Encoding with URL and Filename Safe Alphabet
    - Base 32 Encoding
    - Base 32 Encoding with Extended Hex Alphabet
    - Base 16 Encoding
- Supports custom alphabet and padding for your Base-N Encoding.
- Does **not** contain **unsafe code**.
- Includes the **simple** and informal **benchmark** subprojects.

Installation
-------
- Download the [assembly][3] and references it manually, or install using [NuGet][2](recommend).

Basic Usage
-----------
1. Adds a reference of the assembly in you project.

        PM> Install-Package WallF.BaseNEncodings

2. Using the WallF.BaseNEncodings **namespace**.

        using WallF.BaseNEncodings;

3. Gets **instance** of encodings.

        // standard encoding(RFC 4648)
        BaseEncoding encoding = BaseEncoding.Base64;
        // custom encoding
        char[] alphabet =  {...};
        char padding = '.';
        BaseEncoding encoding = new Base64Encoding(alphabet, padding, "custom encoding");
        
4. **Converts** by the methods To/FromBaseString, Encode, Decode.

        BaseEncoding encoding = ...;
        byte[] bin = new byte[1024];
        string baseString = encoding.ToBaseString(bin);
        byte[] data = encoding.FromBaseString(baseString);
        // bin and data contains the same elements

Documentation, Simple and Benchmark
------------------------------------
- Documentation comments as the generated XML file(WallF.BaseNEncodings.xml) is included in the package.
- Repository includes the [simple][4] and [benchmark][5] subprojects.


  [1]: http://tools.ietf.org/html/rfc4648
  [2]: http://nuget.org/packages/WallF.BaseNEncodings
  [3]: https://github.com/wallf/BaseNEncodings.Net/releases/download/v1.0.0/BaseNEncodings.Net.v1.0.0.zip
  [4]: https://github.com/wallf/BaseNEncodings.Net/tree/master/Simple
  [5]: https://github.com/wallf/BaseNEncodings.Net/tree/master/Benchmark
  [9]: https://github.com/wallf/java-base-n-encodings