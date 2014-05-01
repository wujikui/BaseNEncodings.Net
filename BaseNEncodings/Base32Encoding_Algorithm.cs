using System;
using WallF.BaseNEncodings.Inner;

namespace WallF.BaseNEncodings
{
    public partial class Base32Encoding : BaseEncoding
    {
        private Base32 b;

        private void InitAlgorithm(char[] alphabet, char padding)
        {
            this.b = new Base32(alphabet, padding);
        }

        /// <summary>
        /// See <see cref="BaseEncoding.GetEncodeCountWithoutArgumentsValidation(int)"/>.
        /// </summary>
        protected override int GetEncodeCountWithoutArgumentsValidation(int length)
        {
            return b.EncodeSize(length);
        }

        /// <summary>
        /// See <see cref="BaseEncoding.EncodeWithoutArgumentsValidation(byte[], int, int)"/>.
        /// </summary>
        protected override char[] EncodeWithoutArgumentsValidation(byte[] bytes, int offset, int length)
        {
            char[] r = new char[b.EncodeSize(length)];
            b.Encode(bytes, offset, length, r, 0, r.Length);
            return r;
        }

        /// <summary>
        /// See <see cref="BaseEncoding.EncodeWithoutArgumentsValidation(byte[], int, int, char[], int)"/>.
        /// </summary>
        /// <exception cref="ArgumentException">output sequence does not have enough capacity</exception>
        protected override int EncodeWithoutArgumentsValidation(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut)
        {
            return b.Encode(bytesIn, offsetIn, lengthIn, charsOut, offsetOut);
        }

        /// <summary>
        /// See <see cref="BaseEncoding.GetDecodeCountWithoutArgumentsValidation(char[], int, int)"/>.
        /// </summary>
        /// <exception cref="FormatException">input sequence is not a valid base sequence</exception>
        protected override int GetDecodeCountWithoutArgumentsValidation(char[] chars, int offset, int length)
        {
            int t;
            return b.DecodeSize(chars, offset, length, out t);
        }

        /// <summary>
        /// See <see cref="BaseEncoding.DecodeWithoutArgumentsValidation(char[], int, int)"/>.
        /// </summary>
        /// <exception cref="FormatException">input sequence is not a valid base sequence</exception>
        protected override byte[] DecodeWithoutArgumentsValidation(char[] chars, int offset, int length)
        {
            int paddingNum;
            byte[] r = new byte[b.DecodeSize(chars, offset, length, out paddingNum)];
            b.Decode(chars, offset, length, r, 0, r.Length, paddingNum);
            return r;
        }

        /// <summary>
        /// See <see cref="BaseEncoding.DecodeWithoutArgumentsValidation(char[], int, int, byte[], int)"/>.
        /// </summary>
        /// <exception cref="FormatException">input sequence is not a valid base sequence</exception>
        /// <exception cref="ArgumentException">output sequence does not have enough capacity</exception>
        protected override int DecodeWithoutArgumentsValidation(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut)
        {
            return b.Decode(charsIn, offsetIn, lengthIn, bytesOut, offsetOut);
        }

        /// <summary>
        /// See <see cref="BaseEncoding.IsValidBaseSequenceWithoutArgumentsValidation(char[], int, int)"/>.
        /// </summary>
        protected override bool IsValidBaseSequenceWithoutArgumentsValidation(char[] chars, int offset, int length)
        {
            return b.IsValidBaseSequence(chars, offset, length);
        }

    }
}