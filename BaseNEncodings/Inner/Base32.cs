using System;
using System.Collections.Generic;

namespace WallF.BaseNEncodings.Inner
{
    internal sealed class Base32
    {
        private readonly char[] charMap;
        private readonly IDictionary<char, int> indexMap;
        private readonly char paddingChar;

        public Base32(char[] alphabet, char padding)
        {
            this.charMap = alphabet;
            this.indexMap = new Dictionary<char, int>(32);
            for (int i = 0; i < 32; i++)
                indexMap.Add(alphabet[i], i);
            this.paddingChar = padding;
        }

        public int EncodeSize(int length)
        {
            return (int)Math.Ceiling(length / 5f) * 8;
        }

        public int Encode(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut, int? lengthOutObj = null)
        {
            // ===============================================================================================================
            //       [1               ][2                      ] [3              ][4                       ][5              ] 
            // 1:{xxx0 1234} 2:{xxx5 6701} 3:{xxx2 3456} 4:{xxx7 0123} 5:{xxx4 5670} 6:{xxx1 2345} 7:{xxx6 7012} 8:{xxx3 4567}
            // ===============================================================================================================
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
            int last = 5, temp = 0, boundIn = offsetIn + lengthIn, boundOut = offsetOut + lengthOut;
            while (offsetIn != boundIn)
            {
                int v = (int)bytesIn[offsetIn++];
                charsOut[offsetOut++] = charMap[temp | (v >> (8 - last))];
                if (last <= 3)
                {
                    charsOut[offsetOut++] = charMap[(v >> (3 - last)) & 0x1F];
                    last += 5;
                }
                temp = (v << (last = last - 3)) & 0x1F;
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
            for (int ir = offset + length, i = ir - 7; i < ir; i++)
            {
                if (chars[i] == paddingChar)
                {
                    paddingNum = ir - i;
                    break;
                }
            }
            return (length - paddingNum) / 8 * 5 + PADDING_VALUES_NUM_MAP[paddingNum];
        }
        private static readonly int[] PADDING_VALUES_NUM_MAP = { 0, 4, 0, 3, 2, 0, 1 };

        public int Decode(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut, int? lengthOutObj = null, int? paddingNumObj = null)
        {
            // ===============================================================================================================
            // 1:{xxx0 1234} 2:{xxx5 6701} 3:{xxx2 3456} 4:{xxx7 0123} 5:{xxx4 5670} 6:{xxx1 2345} 7:{xxx6 7012} 8:{xxx3 4567}
            //       [1               ][2                      ] [3              ][4                       ][5              ] 
            // ===============================================================================================================
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
                if (remain > 5)
                {
                    temp = temp | (v << (remain -= 5));
                }
                else
                {
                    bytesOut[offsetOut++] = (byte)(temp | (v >> (5 - remain)));
                    temp = v << (remain += 3);
                }
            }
            return lengthOut;
        }

        public bool IsValidBaseSequence(char[] chars, int offset, int length)
        {
            if (length == 0) return true;
            if (length % 8 != 0) return false;
            int bound = offset + length; bool findChar = false;
            for (int i = bound - 1, ir = bound - 7; i > ir; i--)
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
            for (int i = bound - 7; i >= offset; i--)
            {
                if (!indexMap.ContainsKey(chars[i]))
                    return false;
            }
            return true;
        }
    }
}