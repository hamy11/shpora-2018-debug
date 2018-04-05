using System;
using System.Collections.Generic;
using JPEG.Images;

namespace JPEG.Utilities
{
    public static class Extensions
    {
        public static void SetPixels(this Matrix matrix, double[,] a, double[,] b, double[,] c,
            int yOffset, int xOffset)
        {
            var height = a.GetLength(0);
            var width = a.GetLength(1);

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                matrix.Pixels[yOffset + y, xOffset + x] =
                    new Pixel((byte) a[y, x], (byte) b[y, x], (byte) c[y, x], matrix);
        }

        public static T[,] GetSubMatrix<T>(this Matrix matrix, int yOffset, int yLength, int xOffset, int xLength,
            Func<Pixel, T> componentSelector)
        {
            var result = new T[yLength, xLength];
            for (var j = 0; j < yLength; j++)
            for (var i = 0; i < xLength; i++)
                result[j, i] = componentSelector(matrix.Pixels[yOffset + j, xOffset + i]);
            return result;
        }

        public static List<T> Join<T>(this List<T>[,] quantinizedMatrix)
        {
            var result = new List<T>();
            for (var i = 0; i < quantinizedMatrix.GetLength(0); i++)
            for (var j = 0; j < quantinizedMatrix.GetLength(1); j++)
            {
                result.AddRange(quantinizedMatrix[i, j]);
            }
            return result;
        }

        public static T[] Join<T>(this T[,] quantinizedMatrix)
        {
            var result = new List<T>();
            for (var i = 0; i < quantinizedMatrix.GetLength(0); i++)
            for (var j = 0; j < quantinizedMatrix.GetLength(1); j++)
            {
                result.Add(quantinizedMatrix[i, j]);
            }
            return result.ToArray();
        }

        public static T[] ReadFrom<T>(this T[] source, int pos, int length)
        {
            var result = new T[length];
            for (var i = pos; i < pos + length; i++)
            {
                result[i - pos] = source[i];
            }
            return result;
        }

        public static void ShiftValues(this double[,] subMatrix, int shiftValue)
        {
            var height = subMatrix.GetLength(0);
            var width = subMatrix.GetLength(1);

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                subMatrix[y, x] = subMatrix[y, x] + shiftValue;
        }
    }
}