using System;
using System.Collections.Generic;

namespace WallF.BaseNEncodings.Inner
{
    internal sealed class Base16
    {
        private readonly char[] charMap;
        private readonly IDictionary<char, int> indexMap;

        public Base16(char[] alphabet)
        {
            this.charMap = alphabet;
            this.indexMap = new Dictionary<char, int>(16);
            for (int i = 0; i < 16; i++)
                indexMap.Add(alphabet[i], i);
        }

        public int EncodeSize(int length)
        {
            return length * 2;
        }

        public int Encode(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut, int? lengthOutObj = null)
        {
            // ===========================
            //         [1               ] 
            // 1:{xxxx 0123} 2:{xxxx 4567}
            // ===========================
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
            int boundIn = offsetIn + lengthIn;
            while (offsetIn != boundIn)
            {
                int v = (int)bytesIn[offsetIn++];
                charsOut[offsetOut++] = charMap[v >> 4];
                charsOut[offsetOut++] = charMap[v & 0x0F];
            }
            return lengthOut;
        }

        public int DecodeSize(int length)
        {
            if (length % 2 != 0) throw new FormatException("input sequence is not a valid base sequence");
            return length / 2;
        }

        public int Decode(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut, int? lengthOutObj = null)
        {
            // ===========================
            // 1:{xxxx 0123} 2:{xxxx 4567}
            //         [1               ] 
            // ===========================
            if (lengthIn == 0) return 0;
            int lengthOut;
            if (lengthOutObj != null)
            {
                lengthOut = lengthOutObj.Value;
            }
            else
            {
                lengthOut = DecodeSize(lengthIn);
                if (bytesOut.Length - offsetOut < lengthOut) throw new ArgumentException("output sequence does not have enough capacity");
            }
            int boundIn = offsetIn + lengthIn;
            while (offsetIn != boundIn)
            {
                int oa, ob;
                if (indexMap.TryGetValue(charsIn[offsetIn++], out oa) && indexMap.TryGetValue(charsIn[offsetIn++], out ob))
                    bytesOut[offsetOut++] = (byte)(oa << 4 | ob);
                else
                    throw new FormatException("input sequence is not a valid base sequence");
            }
            return lengthOut;
        }

        public bool IsValidBaseSequence(char[] chars, int offset, int length)
        {
            if (length % 2 != 0) return false;
            int bound = offset + length;
            while (offset != bound)
            {
                if (!indexMap.ContainsKey(chars[offset++]))
                    return false;
            }
            return true;
        }
    }
}