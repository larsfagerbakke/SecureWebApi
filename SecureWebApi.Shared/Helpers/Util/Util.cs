using System;
using System.Collections.Generic;
using System.Text;

namespace SecureWebApi.Shared.Helpers.Util
{
    public class Util
    {
        public static Random Random = new Random();

        public static char[] Chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };

        public static int[] Numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

        public static string GenerateRandomString(int length)
        {
            var ret = string.Empty;

            while (ret.Length < length)
                ret += Chars[Util.Random.Next(0, Chars.Length - 1)];

            return ret;
        }
    }
}
