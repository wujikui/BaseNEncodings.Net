using System;
using WallF.BaseNEncodings.Util;

namespace WallF.BaseNEncodings
{
    /// <summary>
    /// Represents a Base-N data encoding.
    /// <para>Defines the general properties and methods of BaseEncoding.</para>
    /// <para>Provides easy access to standard encodings of <a href="http://tools.ietf.org/rfc/rfc4648.txt">RFC 4648</a>.</para>
    /// </summary>
    public abstract class BaseEncoding
    {

        #region static get properties of the standard encodings

        private static BaseEncoding base16Encoding;
        private static BaseEncoding base32Encoding;
        private static BaseEncoding base32HexEncoding;
        private static BaseEncoding base64Encoding;
        private static BaseEncoding base64SafeEncoding;

        /// <summary>
        /// Gets a standard encoding for the Base16 Data Encoding(RFC 4648).
        /// <para>See <see cref="Base16Encoding"/>.</para>
        /// </summary>
        public static BaseEncoding Base16
        {
            get
            {
                if (base16Encoding == null) base16Encoding = new Base16Encoding();
                return base16Encoding;
            }
        }
        /// <summary>
        /// Gets a standard encoding for the Base32 Data Encoding(RFC 4648).
        /// <para>See <see cref="Base32Encoding"/>.</para>
        /// </summary>
        public static BaseEncoding Base32
        {
            get
            {
                if (base32Encoding == null) base32Encoding = new Base32Encoding();
                return base32Encoding;
            }
        }
        /// <summary>
        /// Gets a standard encoding for the Base32 Data Encoding(RFC 4648) with Extended Hex Alphabet.
        /// <para>See <see cref="Base32HexEncoding"/>.</para>
        /// </summary>
        public static BaseEncoding Base32Hex
        {
            get
            {
                if (base32HexEncoding == null) base32HexEncoding = new Base32HexEncoding();
                return base32HexEncoding;
            }
        }
        /// <summary>
        /// Gets a standard encoding for the Base64 Data Encoding(RFC 4648).
        /// <para>See <see cref="Base64Encoding"/>.</para>
        /// </summary>
        public static BaseEncoding Base64
        {
            get
            {
                if (base64Encoding == null) base64Encoding = new Base64Encoding();
                return base64Encoding;
            }
        }
        /// <summary>
        /// Gets a standard encoding for the Base64 Data Encoding(RFC 4648) with URL and Filename Safe Alphabet.
        /// <para>See <see cref="Base64SafeEncoding"/>.</para>
        /// </summary>
        public static BaseEncoding Base64Safe
        {
            get
            {
                if (base64SafeEncoding == null) base64SafeEncoding = new Base64SafeEncoding();
                return base64SafeEncoding;
            }
        }

        #endregion


        #region abstract general properties

        /// <summary>
        /// When overridden in a derived class, gets the human-readable description of the current encoding.
        /// </summary>
        public abstract string EncodingName { get; }
        /// <summary>
        /// When overridden in a derived class, gets the being used alphabet of the current encoding.
        /// </summary>
        public abstract char[] Alphabet { get; }
        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether padding character is required of the current encoding.
        /// </summary>
        public abstract bool IsPaddingRequired { get; }
        /// <summary>
        /// When overridden in a derived class, gets the being used padding character of the current encoding.
        /// </summary>
        public abstract char PaddingCharacter { get; }

        #endregion


        #region virtual, easy accessibility

        /// <summary>
        /// When overridden in a derived class, converts all the bytes in the specified byte array to its equivalent string representation that is encoded with base-n digits by current encoding.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to convert.</param>
        /// <returns>The string representation, in base-n, of contents of the specified byte array.</returns>
        /// <exception cref="ArgumentNullException">bytes is null</exception>
        public virtual string ToBaseString(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes", "bytes is null");
            return new string(EncodeWithoutArgumentsValidation(bytes, 0, bytes.Length));
        }

        /// <summary>
        /// When overridden in a derived class, converts all the bytes in the specified byte array to its equivalent string representation that is encoded with base-n digits by current encoding.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to convert.</param>
        /// <param name="offset">The index of the first byte to convert.</param>
        /// <param name="length">The number of bytes to convert.</param>
        /// <returns>The string representation, in base-n, of contents of the specified byte array.</returns>
        /// <exception cref="ArgumentNullException">bytes is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset and length can't reference an effective tuple of bytes</exception>
        public virtual string ToBaseString(byte[] bytes, int offset, int length)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes", "bytes is null");
            if (!ArrayFunctions.ValidationInterval(bytes, offset, length))
                throw new ArgumentOutOfRangeException("offset or length", "offset and length can't reference an effective tuple of bytes");
            return new string(EncodeWithoutArgumentsValidation(bytes, offset, length));
        }

        /// <summary>
        /// When overridden in a derived class, converts all the characters in the specified string to its equivalent binary data representation that is decoded with base-n string by current encoding.
        /// </summary>
        /// <param name="s">The string containing the characters to convert.</param>
        /// <returns>A byte array containing the results that is equivalent to s.</returns>
        public byte[] FromBaseString(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s", "s is null");
            return DecodeWithoutArgumentsValidation(s.ToCharArray(), 0, s.Length);
        }

        /// <summary>
        /// When overridden in a derived class, converts all the characters in the specified string to its equivalent binary data representation that is decoded with base-n string by current encoding.
        /// <para>Safety version of <see cref="FromBaseString(string)"/>, it ignores the execution of possible exceptions.</para>
        /// </summary>
        /// <param name="s">The string containing the characters to convert.</param>
        /// <param name="bytes">The byte array containing the results that is equivalent to s, as an output parameter.</param>
        /// <returns>Returns ture if converted success.</returns>
        public virtual bool TryFromBaseString(string s, out byte[] bytes)
        {
            if (s == null)
                throw new ArgumentNullException("s", "s is null");
            try
            {
                bytes = DecodeWithoutArgumentsValidation(s.ToCharArray(), 0, s.Length);
                return true;
            }
            catch { bytes = null; return false; }
        }

        #endregion


        #region virtual, typical encode methods

        /// <summary>
        /// When overridden in a derived class, calculates the number of characters produced by encoding the sequence of bytes specified length from byte array.
        /// </summary>
        /// <param name="length">The number of bytes to encode.</param>
        /// <returns>The number of characters produced by encoding the sequence of bytes specified length.</returns>
        /// <exception cref="ArgumentOutOfRangeException">length is less than 0</exception>
        public virtual int GetEncodeCount(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "length is less than 0");
            return GetEncodeCountWithoutArgumentsValidation(length);
        }

        /// <summary>
        /// When overridden in a derived class, encodes all the bytes in the specified byte array into a set of characters.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to encode.</param>
        /// <returns>A character array containing the results of encoding the specified sequence of bytes.</returns>
        /// <exception cref="ArgumentNullException">bytes is null</exception>
        public virtual char[] Encode(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes", "bytes is null");
            return EncodeWithoutArgumentsValidation(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// When overridden in a derived class, encodes all the bytes in the specified byte array into a set of characters.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to encode.</param>
        /// <param name="offset">The index of the first byte to encode.</param>
        /// <param name="length">The number of bytes to encode.</param>
        /// <returns>A character array containing the results of encoding the specified sequence of bytes.</returns>
        /// <exception cref="ArgumentNullException">bytes is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset and length can't reference an effective tuple of bytes</exception>
        public virtual char[] Encode(byte[] bytes, int offset, int length)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes", "bytes is null");
            if (!ArrayFunctions.ValidationInterval(bytes, offset, length))
                throw new ArgumentOutOfRangeException("offset or length", "offset and length can't reference an effective tuple of bytes");
            return EncodeWithoutArgumentsValidation(bytes, offset, length);
        }

        /// <summary>
        /// When overridden in a derived class, encodes a sequence of bytes from the specified byte array into the specified character array.
        /// </summary>
        /// <param name="bytesIn">The byte array containing the sequence of bytes to encode.</param>
        /// <param name="offsetIn">The index of the first byte to encode.</param>
        /// <param name="lengthIn">The number of bytes to encode.</param>
        /// <param name="charsOut">The character array to contain the resulting set of characters.</param>
        /// <param name="offsetOut">The index at which to start writing the resulting set of characters.</param>
        /// <returns>The actual number of characters written into chars.</returns>
        /// <exception cref="ArgumentNullException">bytesIn or charsOut is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offsetIn and lengthIn can't reference an effective tuple of bytesIn, or offsetOut is not an index of charsOut</exception>
        public virtual int Encode(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut)
        {
            if (bytesIn == null)
                throw new ArgumentNullException("bytesIn", "bytesIn is null");
            if (!ArrayFunctions.ValidationInterval(bytesIn, offsetIn, lengthIn))
                throw new ArgumentOutOfRangeException("offsetIn or lengthIn", "offsetIn and lengthIn can't reference an effective tuple of bytesIn");
            if (charsOut == null)
                throw new ArgumentNullException("charsIn", "charsIn is null");
            if (!ArrayFunctions.ValidationInterval(charsOut, offsetOut))
                throw new ArgumentOutOfRangeException("offsetOut", "offsetOut is not an index of charsOut");
            return EncodeWithoutArgumentsValidation(bytesIn, offsetIn, lengthIn, charsOut, offsetOut);
        }

        #endregion


        #region virtual, typical decode methods

        /// <summary>
        /// When overridden in a derived class, calculates the number of bytes produced by decoding a set of characters from the specified character array.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to decode.</param>
        /// <param name="offset">The index of the first character to decode.</param>
        /// <param name="length">The number of characters to decode.</param>
        /// <returns>The number of bytes produced by decoding the specified characters.</returns>
        /// <exception cref="ArgumentNullException">chars is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset and length can't reference an effective tuple of chars</exception>
        public virtual int GetDecodeCount(char[] chars, int offset, int length)
        {
            if (chars == null)
                throw new ArgumentNullException("chars", "chars is null");
            if (!ArrayFunctions.ValidationInterval(chars, offset, length))
                throw new ArgumentOutOfRangeException("offset or length", "offset and length can't reference an effective tuple of chars");
            return GetDecodeCountWithoutArgumentsValidation(chars, offset, length);
        }

        /// <summary>
        /// When overridden in a derived class, decodes all the characters in the specified character array into a sequence of bytes.
        /// </summary>
        /// <param name="chars">The character array containing the characters to decode.</param>
        /// <returns>A byte array containing the results of decoding the specified set of characters.</returns>
        /// <exception cref="ArgumentNullException">chars is null</exception>
        public virtual byte[] Decode(char[] chars)
        {
            if (chars == null)
                throw new ArgumentNullException("chars", "chars is null");
            return DecodeWithoutArgumentsValidation(chars, 0, chars.Length);
        }

        /// <summary>
        /// When overridden in a derived class, decodes a set of characters from the specified character array into a sequence of bytes.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to decode.</param>
        /// <param name="offset">The index of the first character to decode.</param>
        /// <param name="length">The number of characters to decode.</param>
        /// <returns>A byte array containing the results of decoding the specified set of characters.</returns>
        /// <exception cref="ArgumentNullException">chars is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset and length can't reference an effective tuple of chars</exception>
        public virtual byte[] Decode(char[] chars, int offset, int length)
        {
            if (chars == null)
                throw new ArgumentNullException("chars", "chars is null");
            if (!ArrayFunctions.ValidationInterval(chars, offset, length))
                throw new ArgumentOutOfRangeException("offset or length", "offset and length can't reference an effective tuple of chars");
            return DecodeWithoutArgumentsValidation(chars, offset, length);
        }

        /// <summary>
        /// When overridden in a derived class, decodes a set of characters from the specified character array into the specified byte array.
        /// </summary>
        /// <param name="charsIn">The character array containing the set of characters to decode.</param>
        /// <param name="offsetIn">The index of the first character to decode.</param>
        /// <param name="lengthIn">The number of characters to decode.</param>
        /// <param name="bytesOut">The byte array to contain the resulting sequence of bytes.</param>
        /// <param name="offsetOut">The index at which to start writing the resulting sequence of bytes.</param>
        /// <returns>The actual number of bytes written into bytes.</returns>
        /// <exception cref="ArgumentNullException">charsIn or bytesOut is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offsetIn and lengthIn can't reference an effective tuple of charsIn, or offsetOut is not an index of bytesOut</exception>
        public virtual int Decode(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut)
        {
            if (charsIn == null)
                throw new ArgumentNullException("charsIn", "charsIn is null");
            if (!ArrayFunctions.ValidationInterval(charsIn, offsetIn, lengthIn))
                throw new ArgumentOutOfRangeException("offsetIn or lengthIn", "offsetIn and lengthIn can't reference an effective tuple of charsIn");
            if (bytesOut == null)
                throw new ArgumentNullException("bytesOut", "bytesOut is null");
            if (!ArrayFunctions.ValidationInterval(bytesOut, offsetOut))
                throw new ArgumentOutOfRangeException("offsetOut", "offsetOut is not an index of bytesOut");
            return DecodeWithoutArgumentsValidation(charsIn, offsetIn, lengthIn, bytesOut, offsetOut);
        }

        #endregion


        #region virtual, utility methods

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether a set of characters from the specified character array is actually valid by current encoding.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to validate.</param>
        /// <param name="offset">The index of the first character to validate.</param>
        /// <param name="length">The number of characters to validate.</param>
        /// <returns>Returns true if the specified character array is valid.</returns>
        /// <exception cref="ArgumentNullException">chars is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset and length can't reference an effective tuple of chars</exception>
        public virtual bool IsValidBaseSequence(char[] chars, int offset, int length)
        {
            if (chars == null)
                throw new ArgumentNullException("chars", "chars is null");
            if (!ArrayFunctions.ValidationInterval(chars, offset, length))
                throw new ArgumentOutOfRangeException("offset or length", "offset and length can't reference an effective tuple of chars");
            return IsValidBaseSequenceWithoutArgumentsValidation(chars, offset, length);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether a set of characters from the specified string is actually valid by current encoding.
        /// </summary>
        /// <param name="s">The string containing the set of characters to validate.</param>
        /// <returns>Returns true if the specified character array is valid.</returns>
        /// <exception cref="ArgumentNullException">s is null</exception>
        public virtual bool IsValidBaseString(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s", "s is null");
            return IsValidBaseSequenceWithoutArgumentsValidation(s.ToCharArray(), 0, s.Length);
        }

        #endregion


        #region abstract, core methods

        /// <summary>
        /// When overridden in a derived class, calculates the number of characters produced by encoding the sequence of bytes specified length from byte array.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="length">The number of bytes to encode.</param>
        /// <returns>The number of characters produced by encoding the sequence of bytes specified length.</returns>
        protected abstract int GetEncodeCountWithoutArgumentsValidation(int length);

        /// <summary>
        /// When overridden in a derived class, encodes all the bytes in the specified byte array into a set of characters.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to encode.</param>
        /// <param name="offset">The index of the first byte to encode.</param>
        /// <param name="length">The number of bytes to encode.</param>
        /// <returns>A character array containing the results of encoding the specified sequence of bytes.</returns>
        protected abstract char[] EncodeWithoutArgumentsValidation(byte[] bytes, int offset, int length);

        /// <summary>
        /// When overridden in a derived class, encodes a sequence of bytes from the specified byte array into the specified character array.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="bytesIn">The byte array containing the sequence of bytes to encode.</param>
        /// <param name="offsetIn">The index of the first byte to encode.</param>
        /// <param name="lengthIn">The number of bytes to encode.</param>
        /// <param name="charsOut">The character array to contain the resulting set of characters.</param>
        /// <param name="offsetOut">The index at which to start writing the resulting set of characters.</param>
        /// <returns>The actual number of characters written into chars.</returns>
        protected abstract int EncodeWithoutArgumentsValidation(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut);

        /// <summary>
        /// When overridden in a derived class, calculates the number of bytes produced by decoding a set of characters from the specified character array.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to decode.</param>
        /// <param name="offset">The index of the first character to decode.</param>
        /// <param name="length">The number of characters to decode.</param>
        /// <returns>The number of bytes produced by decoding the specified characters.</returns>
        protected abstract int GetDecodeCountWithoutArgumentsValidation(char[] chars, int offset, int length);

        /// <summary>
        /// When overridden in a derived class, decodes a set of characters from the specified character array into a sequence of bytes.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to decode.</param>
        /// <param name="offset">The index of the first character to decode.</param>
        /// <param name="length">The number of characters to decode.</param>
        /// <returns>A byte array containing the results of decoding the specified set of characters.</returns>
        protected abstract byte[] DecodeWithoutArgumentsValidation(char[] chars, int offset, int length);

        /// <summary>
        /// When overridden in a derived class, decodes a set of characters from the specified character array into the specified byte array.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="charsIn">The character array containing the set of characters to decode.</param>
        /// <param name="offsetIn">The index of the first character to decode.</param>
        /// <param name="lengthIn">The number of characters to decode.</param>
        /// <param name="bytesOut">The byte array to contain the resulting sequence of bytes.</param>
        /// <param name="offsetOut">The index at which to start writing the resulting sequence of bytes.</param>
        /// <returns>The actual number of bytes written into bytes.</returns>
        protected abstract int DecodeWithoutArgumentsValidation(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut);

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether a set of characters from the specified character array is actually valid by current encoding.
        /// <para>No need to verify the correctness of the arguments.</para>
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to validate.</param>
        /// <param name="offset">The index of the first character to validate.</param>
        /// <param name="length">The number of characters to validate.</param>
        /// <returns>Returns true if the specified character array is valid.</returns>
        protected abstract bool IsValidBaseSequenceWithoutArgumentsValidation(char[] chars, int offset, int length);

        #endregion

    }
}