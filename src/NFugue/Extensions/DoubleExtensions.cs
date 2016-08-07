using System;

namespace NFugue.Extensions
{
    public static class DoubleExtensions
    {
        private static readonly double[] powersOf10 = { 1e0, 1e1, 1e2, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9, 1e10 };

        public static double Truncate(this double x, int precision)
        {
            if (precision < 0)
                throw new ArgumentException();
            if (precision == 0)
                return Math.Truncate(x);
            double m = precision >= powersOf10.Length ? Math.Pow(10, precision) : powersOf10[precision];
            return Math.Truncate(x * m) / m;
        }
    }
}