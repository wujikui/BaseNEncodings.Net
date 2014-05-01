using System;
using System.Collections.Generic;
using System.Text;

namespace WallF.BaseNEncodings.Util
{
    internal static class ArrayFunctions
    {
        /// <remarks>.Net Framework does not apply inline optimization of generics</remarks>
        public static bool ValidationInterval(byte[] arr, int offset)
        {
            return offset >= 0 && offset <= arr.Length;
        }
        /// <remarks>.Net Framework does not apply inline optimization of generics</remarks>
        public static bool ValidationInterval(char[] arr, int offset)
        {
            return offset >= 0 && offset <= arr.Length;
        }

        /// <remarks>.Net Framework does not apply inline optimization of generics</remarks>
        public static bool ValidationInterval(byte[] arr, int offset, int length)
        {
            return
                offset >= 0
                && length >= 0
                && offset + length <= arr.Length;
        }
        /// <remarks>.Net Framework does not apply inline optimization of generics</remarks>
        public static bool ValidationInterval(char[] arr, int offset, int length)
        {
            return
                offset >= 0
                && length >= 0
                && offset + length <= arr.Length;
        }

        public static bool IsArrayDuplicate(char[] chars)
        {
            // using HashSet in .Net 3.5+
            Dictionary<char, object> dict = new Dictionary<char, object>(chars.Length);
            for (var i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (dict.ContainsKey(c))
                    return true;
                else
                    dict.Add(c, null);
            }
            return false;
        }

        public static bool IsArrayContains(char[] chars, char target)
        {
            for (int i = 0; i < chars.Length; i++)
                if (chars[i] == target)
                    return true;
            return false;
        }
    }
}