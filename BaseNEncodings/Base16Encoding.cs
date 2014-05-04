using System;
using WallF.BaseNEncodings.Util;

namespace WallF.BaseNEncodings
{
    /// <summary>
    /// Represents a Base16 encoding.
    /// <para>Default constructor will create a standard Base16 encoding(RFC 4648).</para>
    /// </summary>
    public partial class Base16Encoding : BaseEncoding
    {
        /// <summary>
        /// Standard Alphabet.
        /// </summary>
        public const string STANDARD_ALPHABET = "0123456789ABCDEF";
        /// <summary>
        /// Default Encoding Name.
        /// </summary>
        public const string DEFAULT_NAME = "Standard Base16 Encoding";

        private readonly char[] alphabet;
        private readonly string encodingName;

        /// <summary>
        /// Initializes a new instance that is a standard Base16 encoding(<a href="http://tools.ietf.org/rfc/rfc4648.txt">RFC 4648</a>).
        /// </summary>
        public Base16Encoding() : this(STANDARD_ALPHABET.ToCharArray(), DEFAULT_NAME, false) { }

        /// <summary>
        /// Initializes a new instance of the Base16Encoding class. Parameters specify the alphabet of encoding.
        /// </summary>
        /// <param name="alphabet">Alphabet for current encoding.</param>
        /// <exception cref="ArgumentNullException">alphabet is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">size of alphabet is not 16</exception>
        /// <exception cref="ArgumentException">alphabet contains duplicated items</exception>
        public Base16Encoding(char[] alphabet) : this(alphabet, "Customized Base16 Encoding", true) { }

        /// <summary>
        /// Initializes a new instance of the Base16Encoding class. Parameters specify the alphabet and the name of encoding.
        /// </summary>
        /// <param name="alphabet">Alphabet for current encoding.</param>
        /// <param name="encodingName">Name for current encoding.</param>
        /// /// <exception cref="ArgumentNullException">alphabet or encoodingName is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">size of alphabet is not 16</exception>
        /// <exception cref="ArgumentException">alphabet contains duplicated items</exception>
        public Base16Encoding(char[] alphabet, string encodingName) : this(alphabet, encodingName, true) { }

        internal Base16Encoding(char[] alphabet, string encodingName, bool verify)
        {
            if (verify)
            {
                if (alphabet == null)
                    throw new ArgumentNullException("alphabet", "alphabet is null");
                if (encodingName == null)
                    throw new ArgumentNullException("encodingName", "encodingName is null");
                if (alphabet.Length != 16)
                    throw new ArgumentOutOfRangeException("alphabet", "size of alphabet is not 16");
                if (ArrayFunctions.IsArrayDuplicate(alphabet))
                    throw new ArgumentException("alphabet", "alphabet contains duplicated items");
            }
            this.alphabet = (char[])alphabet.Clone();
            this.encodingName = encodingName;
            this.InitAlgorithm(this.alphabet);
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
        /// Return value is always false, because of padding is not required for Base16 Encoding.
        /// </summary>
        public override bool IsPaddingRequired
        {
            get { return false; }
        }

        /// <summary>
        /// Return value is always default character, because of padding is not required for Base16 Encoding.
        /// </summary>
        public override char PaddingCharacter
        {
            get { return default(char); }
        }

    }
}