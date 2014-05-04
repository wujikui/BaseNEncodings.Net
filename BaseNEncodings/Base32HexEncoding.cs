
namespace WallF.BaseNEncodings
{
    /// <summary>
    /// Defines a Base32 encoding with Extended Hex Alphabet.
    /// </summary>
    public sealed class Base32HexEncoding : Base32Encoding
    {
        /// <summary>
        /// Standard Alphabet.
        /// </summary>
        public new const string STANDARD_ALPHABET = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
        /// <summary>
        /// Standard Padding.
        /// </summary>
        public new const char STANDARD_PADDING = '=';
        /// <summary>
        /// Default Encoding Name.
        /// </summary>
        public new const string DEFAULT_NAME = "Standard Base32 Encoding with Extended Hex Alphabet";

        /// <summary>
        /// Initializes a new instance that is a standard Base32 encoding(<a href="http://tools.ietf.org/rfc/rfc4648.txt">RFC 4648</a>) with Extended Hex Alphabet.
        /// </summary>
        public Base32HexEncoding() : base(STANDARD_ALPHABET.ToCharArray(), STANDARD_PADDING, DEFAULT_NAME, false) { }
    }
}