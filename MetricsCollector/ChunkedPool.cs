using System;
using System.Collections.Generic;

namespace MetricsCollector
{
    public class ChunkedPool<T>
    {
        private readonly ChunkedPoolConfig<T> poolConfig;
        private readonly object sync = new object();
        private readonly Stack<T> smallObjects = new Stack<T>();
        private readonly Stack<T> mediumObjects = new Stack<T>();
        private readonly Stack<T> largeObjects = new Stack<T>();

        public ChunkedPool(ChunkedPoolConfig<T> poolConfig)
        {
            this.poolConfig = poolConfig;
        }

        public void Release(T item)
        {
            poolConfig.Cleanup(item);
            var size = poolConfig.GetSize(item);
            var bucket = GetBucket(size);
            lock (sync)
                bucket.Push(item);
        }

        public Disposable<T> Acquire(long size)
        {
            var bucket = GetBucket(size);
            return AcquireFrom(bucket, size);
        }
        
        private Disposable<T> AcquireFrom(Stack<T> collection, long size)
        {
            T item;
            lock(sync)
                return new Disposable<T>(collection.TryTake(out item) ? item : poolConfig.CreateInstance(size), Release);
        }

        private Stack<T> GetBucket(long size)
        {
            var sizeCategory = poolConfig.GetSizeCategory(size);
            
            switch (sizeCategory)
            {
                case SizeCategory.Small:
                    return smallObjects;
                case SizeCategory.Medium:
                    return mediumObjects;
                case SizeCategory.Large:
                    return largeObjects;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sizeCategory), sizeCategory, null);
            }
        }

    }
}