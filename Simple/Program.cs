using System;
using System.Text;

namespace WallF.BaseNEncodings.Sample
{
    class Program
    {
        // see http://tools.ietf.org/html/rfc4648#section-10
        static readonly string[] TEST_VECTORS = { "", "f", "fo", "foo", "foob", "fooba", "foobar" };

        static void Main(string[] args)
        {
            BaseEncoding b64 = BaseEncoding.Base64
                            , b64Safe = BaseEncoding.Base64Safe
                            , b32 = BaseEncoding.Base32
                            , b32Hex = BaseEncoding.Base32Hex
                            , b16 = BaseEncoding.Base16;
            foreach (string vector in TEST_VECTORS)
            {
                Usage1("Base64", b64, vector);
                Usage2("Base64Safe", b64Safe, vector);
                Usage1("Base32", b32, vector);
                Usage3("Base32Hex", b32Hex, vector);
                Usage2("Base16", b16, vector);
            }
            Console.ReadKey();
        }

        // ========= USAGE 1 =========
        // string BaseEncoding.ToBaseString(byte[] bytes)
        // byte[] BaseEncoding.FromBaseString(string s)
        static void Usage1(string testName, BaseEncoding encoding, string testVector)
        {
            byte[] origin = Encoding.Default.GetBytes(testVector);
            string baseString = encoding.ToBaseString(origin);
            byte[] bytes = encoding.FromBaseString(baseString);
            Console.WriteLine("[" + testName + "]\tVector: " + testVector + "\tBaseString: "
                + baseString + "\t" + (ArrayEquals(origin, bytes) ? "Success" : "failed"));
        }

        // ========= USAGE 2 =========
        // char[] BaseEncoding.Encode(byte[] bytes, int offset, int length)
        // byte[] BaseEncoding.Decode(char[] chars, int offset, int length)
        static void Usage2(string testName, BaseEncoding encoding, string testVector)
        {
            byte[] origin = Encoding.Default.GetBytes(testVector);
            char[] baseChars = encoding.Encode(origin, 0, origin.Length);
            byte[] bytes = encoding.Decode(baseChars, 0, baseChars.Length);
            Console.WriteLine("[" + testName + "]\tVector: " + testVector + "\tBaseString: "
                + new string(baseChars) + "\t" + (ArrayEquals(origin, bytes) ? "Success" : "failed"));
        }

        // ========= USAGE 3 =========
        // int BaseEncoding.GetEncodeCount(int length)
        // int BaseEncoding.Encode(byte[] bytesIn, int offsetIn, int lengthIn, char[] charsOut, int offsetOut)
        // int BaseEncoding.GetDecodeCount(char[] chars, int offset, int length)
        // int BaseEncoding.Decode(char[] charsIn, int offsetIn, int lengthIn, byte[] bytesOut, int offsetOut)
        static void Usage3(string testName, BaseEncoding encoding, string testVector)
        {
            int encodeOffset = 99, decodeOffset = 199;
            byte[] origin = Encoding.Default.GetBytes(testVector);
            byte[] originBuffer = WrapArray<byte>(origin, 33, 44);
            char[] baseCharsBuffer = new char[encoding.GetEncodeCount(origin.Length) + encodeOffset * 2];
            // == encode
            int encodeNum = encoding.Encode(originBuffer, 33, origin.Length, baseCharsBuffer, encodeOffset);
            // ==
            byte[] binBuffer = new byte[encoding.GetDecodeCount(baseCharsBuffer, encodeOffset, encodeNum) + decodeOffset * 2];
            // == decode
            int decodeNum = encoding.Decode(baseCharsBuffer, encodeOffset, encodeNum, binBuffer, decodeOffset);
            // ====== result
            char[] baseChars = SubArray<char>(baseCharsBuffer, encodeOffset, encodeNum);
            byte[] binData = SubArray<byte>(binBuffer, decodeOffset, decodeNum);
            // == output
            Console.WriteLine("[" + testName + "]\tVector: " + testVector + "\tBaseString: "
                + new string(baseChars) + "\t" + (ArrayEquals(origin, binData) ? "Success" : "failed"));
        }


        static bool ArrayEquals(byte[] arr1, byte[] arr2)
        {
            if (arr1 == null || arr2 == null) return false;
            if (arr1.Length != arr2.Length) return false;
            for (int i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;
            return true;
        }
        static T[] SubArray<T>(T[] arr, int index, int count)
        {
            T[] r = new T[count];
            Array.Copy(arr, index, r, 0, count);
            return r;
        }
        static T[] WrapArray<T>(T[] arr, int leftPadNum, int rightPadNum)
        {
            T[] r = new T[arr.Length + leftPadNum + rightPadNum];
            Array.Copy(arr, 0, r, leftPadNum, arr.Length);
            return r;
        }
    }
}