using System;

namespace MetricsCollector
{
    public class ChunkedPoolConfig<T>
    {
        public Func<long, T> CreateInstance { get; }
        public Func<long, SizeCategory> GetSizeCategory { get; }
        public Action<T> Cleanup { get; }
        public Func<T, long> GetSize { get; }

        public ChunkedPoolConfig(
            Func<T, long> getSize,
            Func<long, SizeCategory> getSizeCategory,
            Func<long, T> createInstance=null,
            Action<T> cleanup=null)
        {
            if (getSize == null)
                throw new ArgumentNullException(nameof(getSize));
            if (getSizeCategory == null)
                throw new ArgumentNullException(nameof(getSizeCategory));
            GetSizeCategory = getSizeCategory;
            GetSize = getSize;
            Cleanup = cleanup ?? delegate { };
            CreateInstance = createInstance ?? (_ => Activator.CreateInstance<T>());
        }
    }
}