using System;
using WallF.BaseNEncodings.Util;

namespace WallF.BaseNEncodings
{
    /// <summary>
    /// Represents a Base32 encoding.
    /// <para>Default constructor will create a standard Base32 encoding(RFC 4648).</para>
    /// </summary>
    public partial class Base32Encoding : BaseEncoding
    {
        /// <summary>
        /// Standard Alphabet.
        /// </summary>
        public const string STANDARD_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        /// <summary>
        /// Standard Padding.
        /// </summary>
        public const char STANDARD_PADDING = '=';
        /// <summary>
        /// Default Encoding Name.
        /// </summary>
        public const string DEFAULT_NAME = "Standard Base32 Encoding";

        private readonly char[] alphabet;
        private readonly char padding;
        private readonly string encodingName;

        /// <summary>
        /// Initializes a new instance that is a standard Base32 encoding(<a href="http://tools.ietf.org/rfc/rfc4648.txt">RFC 4648</a>)
        /// </summary>
        public Base32Encoding() : this(STANDARD_ALPHABET.ToCharArray(), STANDARD_PADDING, DEFAULT_NAME, false) { }

        /// <summary>
        /// Initializes a new instance of the Base32Encoding class. Parameters specify the alphabet and the padding character of encoding.
        /// </summary>
        /// <param name="alphabet">Alphabet for current encoding.</param>
        /// <param name="padding">Padding character for current encoding.</param>
        /// <exception cref="ArgumentNullException">alphabet is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">size of alphabet is not 32</exception>
        /// <exception cref="ArgumentException">alphabet contains duplicated items</exception>
        public Base32Encoding(char[] alphabet, char padding) : this(alphabet, padding, "Customized Base32 Encoding", true) { }

        /// <summary>
        /// Initializes a new instance of the Base16Encoding class. Parameters specify the alphabet and the padding character and the name of encoding.
        /// </summary>
        /// <param name="alphabet">Alphabet for current encoding.</param>
        /// <param name="padding">Padding character for current encoding.</param>
        /// <param name="encodingName">Name for current encoding.</param>
        /// /// <exception cref="ArgumentNullException">alphabet or encoodingName is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">size of alphabet is not 32</exception>
        /// <exception cref="ArgumentException">alphabet contains duplicated items, or padding already existed in alphabet</exception>
        public Base32Encoding(char[] alphabet, char padding, string encodingName) : this(alphabet, padding, encodingName, true) { }

        internal Base32Encoding(char[] alphabet, char padding, string encodingName, bool verify)
        {
            if (verify)
            {
                if (alphabet == null)
                    throw new ArgumentNullException("alphabet", "alphabet is null");
                if (encodingName == null)
                    throw new ArgumentNullException("encodingName", "encodingName is null");
                if (alphabet.Length != 32)
                    throw new ArgumentOutOfRangeException("alphabet", "size of alphabet is not 32");
                if (ArrayFunctions.IsArrayDuplicate(alphabet))
                    throw new ArgumentException("alphabet", "alphabet contains duplicated items");
                if (ArrayFunctions.IsArrayContains(alphabet, padding))
                    throw new ArgumentException("padding", "padding already existed in alphabet");
            }
            this.alphabet = (char[])alphabet.Clone();
            this.padding = padding;
            this.encodingName = encodingName;
            this.InitAlgorithm(this.alphabet, this.padding);
        }

        /// <summary>
        /// Gets the human-readable description of the current encoding.
        /// </summary>
        public override string EncodingName
        {
            get { return encodingName; }
        }

        /// <summary>
        /// Gets the being used alphabet of the current encoding.
        /// </summary>
        public override char[] Alphabet
        {
            get { return (char[])alphabet.Clone(); }
        }

        /// <summary>
        /// Return values is always true for the Base32 Encoding.
        /// </summary>
        public override bool IsPaddingRequired
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the being used padding character of the current encoding.
        /// </summary>
        public override char PaddingCharacter
        {
            get { return padding; }
        }

    }
}