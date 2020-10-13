﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecureWebApi.Shared.Helpers.Util
{
    public class Math
    {
        public static int GenerateNumberBetween(int min, int max)
        {
            return min + (int)((max - min) * Util.Random.NextDouble());
        }
    }
}
