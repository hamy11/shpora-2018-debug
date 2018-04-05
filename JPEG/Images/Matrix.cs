using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = JPEG.Images.PixelFormat;

namespace JPEG.Images
{
    public class Matrix
    {
        public readonly Pixel[,] Pixels;
        public readonly int Height;
        public readonly int Width;
        public readonly PixelFormat PixelFormat;

        public Matrix(int height, int width, PixelFormat pixelFormat)
        {
            Height = height;
            Width = width;
            PixelFormat = pixelFormat;
            Pixels = new Pixel[height, width];
        }

        public Matrix(int height, int width,PixelFormat pixelFormat, Pixel[,] pixels)
        {
            Height = height;
            Width = width;
            PixelFormat = pixelFormat;
            Pixels = pixels;
        }

        public static explicit operator Matrix(Bitmap bmp)
        {
            var height = bmp.Height - bmp.Height % 8;
            var width = bmp.Width - bmp.Width % 8;
            var matrix = new Matrix(height, width, PixelFormat.RGB);

            unsafe
            {
                RoundMatrix(matrix, bmp, GetPixel);
            }

            return matrix;
        }

        public static explicit operator Bitmap(Matrix matrix)
        {
            var bmp = new Bitmap(matrix.Width, matrix.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                RoundMatrix(matrix, bmp, SetPixel);
            }

            return bmp;
        }

        public unsafe delegate void RoundAction(Matrix matrix, int y, int x, ref byte* innerPtr);

        private static unsafe void RoundMatrix(Matrix matrix, Bitmap bmp, RoundAction roundAction)
        {
            var pixels = matrix.Pixels;
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite,
                bmp.PixelFormat);
            var ptr = (byte*) data.Scan0;
            
            for (var y = 0; y < pixels.GetLength(0); y++)
            {
                var innerPtr = ptr;
                for (var x = 0; x < pixels.GetLength(1); x++)
                {
                    roundAction(matrix, y, x, ref innerPtr);
                }
                ptr += data.Stride;
            }

            bmp.UnlockBits(data);
        }

        private static unsafe void GetPixel(Matrix matrix, int y, int x, ref byte* innerPtr)
        {
            var b = *innerPtr++;
            var g = *innerPtr++;
            var r = *innerPtr++;
            matrix.Pixels[y, x] = new Pixel(r, g, b, matrix);
        }

        private static unsafe void SetPixel(Matrix matrix, int y, int x, ref byte* innerPtr)
        {
            var pixel = matrix.Pixels[y, x];
            *innerPtr++ = (byte) ToByte(pixel.B);
            *innerPtr++ = (byte) ToByte(pixel.G);
            *innerPtr++ = (byte) ToByte(pixel.R);
        }

        public static int ToByte(double d)
        {
            var val = (int) d;
            if (val > byte.MaxValue)
                return byte.MaxValue;

            return val < byte.MinValue ? byte.MinValue : val;
        }
    }
}