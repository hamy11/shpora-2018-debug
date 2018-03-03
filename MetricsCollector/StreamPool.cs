using System.IO;

namespace MetricsCollector
{
    public static class StreamPool
    {
        private static readonly ChunkedPool<MemoryStream> instance
            = new ChunkedPool<MemoryStream>(
                new ChunkedPoolConfig<MemoryStream>(
                    stream => stream.Length,
                    GetSizeCategory,
                    size => new MemoryStream((int) size),
                    stream => stream.Reset()));

        public static Disposable<MemoryStream> Get(long size)
        {
            return instance.Acquire(size);
        }
            
        private static SizeCategory GetSizeCategory(long size)
        {
            if (size < 1024)
                return SizeCategory.Small;
            if (size < 80 * 1024)
                return SizeCategory.Medium;
            return SizeCategory.Large;
        }
    }
}