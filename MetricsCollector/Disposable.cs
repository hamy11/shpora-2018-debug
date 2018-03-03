using System;

namespace MetricsCollector
{
    public sealed class Disposable<T> : IDisposable
    {
        public T Value { get; }
        private readonly Action<T> dispose;

        public Disposable(T value, Action<T> dispose)
        {
            this.Value = value;
            this.dispose = dispose;
        }

        public void Dispose()
        {
            dispose(Value);
        }

        public static implicit operator T(Disposable<T> d)
        {
            return d.Value;
        }
        
        public static implicit operator Disposable<T>(T d)
        {
            return new Disposable<T>(d, _ => { });
        }
    }
}