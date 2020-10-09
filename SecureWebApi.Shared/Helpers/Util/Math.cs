using System;
using System.Collections.Generic;
using System.Text;

namespace SecureWebApi.Shared.Helpers.Util
{
    public class Math
    {
        private static Random rnd = new Random();

        public static int GenerateNumberBetween(int min, int max)
        {
            return min + (int)((max - min) * rnd.NextDouble());
        }
    }
}
