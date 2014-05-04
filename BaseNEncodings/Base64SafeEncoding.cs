
namespace WallF.BaseNEncodings
{
    /// <summary>
    /// Defines a Base64 encoding with URL and Filename Safe Alphabet.
    /// </summary>
    public sealed class Base64SafeEncoding : Base64Encoding
    {
        /// <summary>
        /// Standard Alphabet.
        /// </summary>
        public new const string STANDARD_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
        /// <summary>
        /// Standard Padding.
        /// </summary>
        public new const char STANDARD_PADDING = '=';
        /// <summary>
        /// Default Encoding Name.
        /// </summary>
        public new const string DEFAULT_NAME = "Standard Base64 Encoding with URL and Filename Safe Alphabet";


        /// <summary>
        /// Initializes a new instance that is a standard Base64 encoding(<a href="http://tools.ietf.org/rfc/rfc4648.txt">RFC 4648</a>) with URL and Filename Safe Alphabet.
        /// </summary>
        public Base64SafeEncoding() : base(STANDARD_ALPHABET.ToCharArray(), STANDARD_PADDING, DEFAULT_NAME, false) { }
    }
}