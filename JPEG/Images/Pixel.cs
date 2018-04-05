namespace JPEG.Images
{
    public class Pixel
    {
       public Pixel(byte firstComponent, byte secondComponent, byte thirdComponent, Matrix matrix)
        {
            r = firstComponent;
            g = secondComponent;
            b = thirdComponent;
            this.matrix = matrix;
        }

        private readonly byte r;
        private readonly byte g;
        private readonly byte b;
        private readonly Matrix matrix;

        public double R => matrix.PixelFormat == PixelFormat.RGB ? r : (298.082 * r + 408.583 * Cr) / 256.0 - 222.921;
        public double G => matrix.PixelFormat == PixelFormat.RGB ? g : (298.082 * Y - 100.291 * Cb - 208.120 * Cr) / 256.0 + 135.576;
        public double B => matrix.PixelFormat == PixelFormat.RGB ? b : (298.082 * Y + 516.412 * Cb) / 256.0 - 276.836;

        public double Y => matrix.PixelFormat == PixelFormat.YCbCr ? r : 16.0 + (65.738 * R + 129.057 * G + 24.064 * B) / 256.0;
        public double Cb => matrix.PixelFormat == PixelFormat.YCbCr ? g : 128.0 + (-37.945 * R - 74.494 * G + 112.439 * B) / 256.0;
        public double Cr => matrix.PixelFormat == PixelFormat.YCbCr ? b : 128.0 + (112.439 * R - 94.154 * G - 18.285 * B) / 256.0;
    }
}