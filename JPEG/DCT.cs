using System;

namespace JPEG
{
    public class DCT
    {
        private readonly double[] basisFuncCache;
        private readonly double[] alphaValuesCache;
        private readonly double beta;
        private const byte NumberBitsCount = 3;
        private const int TwicePowered = NumberBitsCount * 2;
        private const int ThricePowered = NumberBitsCount * 3;
        private static double Alpha(int u) => u == 0 ? 1 / Math.Sqrt(2) : 1;

        public DCT(int size)
        {
            alphaValuesCache = new double[size * size];
            beta = 1d / size + 1d / size;
            basisFuncCache = new double[size * size * size * size];
            FillBasisFunctionCache(size);
            FillAlphaValuesCache(size);
         }

        private void FillAlphaValuesCache(int size)
        {
            for (byte i = 0; i < size; i++)
            {
                for (byte j = 0; j < size; j++)
                {
                    alphaValuesCache[i << NumberBitsCount | j] = Alpha(i) * Alpha(j);
                }
            }
        }

        private void FillBasisFunctionCache(int size)
        {
            for (byte u = 0; u < size; u++)
            for (byte v = 0; v < size; v++)
            {
                for (byte x = 0; x < size; x++)
                for (byte y = 0; y < size; y++)
                {
                    basisFuncCache[u << ThricePowered | v << TwicePowered | x << NumberBitsCount | y] =
                        Math.Cos((2d * x + 1d) * u * Math.PI / (2 * size)) *
                        Math.Cos((2d * y + 1d) * v * Math.PI / (2 * size));
                }
            }
        }

        private double BasisFunction(double a, byte u, byte v, byte x, byte y)
        {
            return a * basisFuncCache[u << ThricePowered | v << TwicePowered | x << NumberBitsCount | y];
        }

        public double[,] DCT2D(double[,] input)
        {
            var length = input.GetLength(0);
            var coeffs = new double[length, length];
            for (byte u = 0; u < length; u++)
            for (byte v = 0; v < length; v++)
            {
                var sum = 0.0;
                for (byte x = 0; x < length; x++)
                for (byte y = 0; y < length; y++)
                {
                    sum += BasisFunction(input[x, y], u, v, x, y);
                }
                coeffs[u, v] = sum * beta * alphaValuesCache[u << NumberBitsCount | v];
            }
            return coeffs;
        }

        public void IDCT2D(double[,] coeffs, double[,] output)
        {
            var length = coeffs.GetLength(0);
            for (byte x = 0; x < length; x++)
            for (byte y = 0; y < length; y++)
            {
                var sum = 0.0;
                for (byte u = 0; u < length; u++)
                for (byte v = 0; v < length; v++)
                {
                    sum += BasisFunction(coeffs[u, v], u, v, x, y) * alphaValuesCache[u << NumberBitsCount | v];
                }
                output[x, y] = sum * beta;
            }
        }
    }
}