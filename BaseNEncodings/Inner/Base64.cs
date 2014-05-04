using System;
using System.Collections.Generic;

namespace WallF.BaseNEncodings.Inner
{
    internal sealed class Base64
    {
        private readonly char[] charMap;
        private readonly IDictionary<char, int> indexMap;
        private readonly char paddingChar;

        public Base64(char[] alphabet, char padding)
        {
            this.charMap = alphabet;
            this.indexMap = new Dictionary<char, int>(64);
            for (int i = 0; i < 64; i++)
                indexMap.Add(alphabet[i], i);
            this.paddingChar = padding;
        }

        public int EncodeSize(int length)
        {
            return (int)Math.Ceiling(length / 3f) * 4;
        }

        public int Encode(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut, int? lengthOutObj = null)
        {
            // =======================================================
            //      [1             ] [2             ][3             ] 
            // 1:{xx01 2345} 2:{xx67 0123} 3:{xx45 6701} 4:{xx23 4567}
            // =======================================================
            if (lengthIn == 0) return 0;
            int lengthOut;
            if (lengthOutObj != null)
            {
                lengthOut = lengthOutObj.Value;
            }
            else
            {
                lengthOut = EncodeSize(lengthIn);
                if (charsOut.Length - offsetOut < lengthOut) throw new ArgumentException("output sequence does not have enough capacity");
            }
            int last = 6, temp = 0, boundIn = offsetIn + lengthIn, boundOut = offsetOut + lengthOut;
            while (offsetIn != boundIn)
            {
                int v = (int)bytesIn[offsetIn++];
                charsOut[offsetOut++] = charMap[temp | (v >> (8 - last))];
                if (last <= 2)
                {
                    charsOut[offsetOut++] = charMap[(v >> (2 - last)) & 0x3F];
                    last += 6;
                }
                temp = (v << (last = last - 2)) & 0x3F;
            }
            if (offsetOut != boundOut)
                charsOut[offsetOut++] = charMap[temp];
            while (offsetOut < boundOut)
                charsOut[offsetOut++] = paddingChar;
            return lengthOut;
        }

        public int DecodeSize(char[] chars, int offset, int length, out int paddingNum)
        {
            paddingNum = 0;
            if (length == 0) return 0;
            if (length % 4 != 0) throw new FormatException("input sequence is not a valid base sequence");
            int lastIndex = offset + length - 1;
            if (chars[lastIndex - 1] == paddingChar) paddingNum = 2;
            else if (chars[lastIndex] == paddingChar) paddingNum = 1;
            return (length - paddingNum) / 4 * 3 + PADDING_VALUES_NUM_MAP[paddingNum];
        }
        private static readonly int[] PADDING_VALUES_NUM_MAP = { 0, 2, 1 };

        public int Decode(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut, int? lengthOutObj = null, int? paddingNumObj = null)
        {
            // =======================================================
            // 1:{xx01 2345} 2:{xx67 0123} 3:{xx45 6701} 4:{xx23 4567}
            //      [1             ] [2             ][3             ] 
            // =======================================================
            if (lengthIn == 0) return 0;
            int lengthOut, paddingNum;
            if (lengthOutObj != null && paddingNumObj != null)
            {
                lengthOut = lengthOutObj.Value;
                paddingNum = paddingNumObj.Value;
            }
            else
            {
                lengthOut = DecodeSize(charsIn, offsetIn, lengthIn, out paddingNum);
                if (bytesOut.Length - offsetOut < lengthOut) throw new ArgumentException("output sequence does not have enough capacity");
            }
            int remain = 8, temp = 0, boundIn = offsetIn + lengthIn - paddingNum;
            while (offsetIn != boundIn)
            {
                int v;
                if (!indexMap.TryGetValue(charsIn[offsetIn++], out v)) throw new FormatException("input sequence is not a valid base sequence");
                if (remain > 6)
                {
                    temp = temp | (v << (remain -= 6));
                }
                else
                {
                    bytesOut[offsetOut++] = (byte)(temp | (v >> (6 - remain)));
                    temp = v << (remain += 2);
                }
            }
            return lengthOut;
        }

        public bool IsValidBaseSequence(char[] chars, int offset, int length)
        {
            if (length == 0) return true;
            if (length % 4 != 0) return false;
            int bound = offset + length;
            bool findChar = false;
            for (int i = bound - 1, ir = bound - 3; i > ir; i--)
            {
                char c = chars[i];
                if (c == paddingChar)
                {
                    if (findChar)
                        return false;
                }
                else
                {
                    if (!indexMap.ContainsKey(c))
                        return false;
                    findChar = true;
                }
            }
            for (int i = bound - 3; i >= offset; i--)
            {
                if (!indexMap.ContainsKey(chars[i]))
                    return false;
            }
            return true;
        }
    }
}