using System.Collections.Generic;
using System.IO;

namespace MetricsCollector
{
    public static class Extensions
    {
        public static void Reset(this Stream stream)
        {
            stream.SetLength(0);
        }
        
        public static bool TryTake<T>(this Stack<T> stack, out T item)
        {
            if (stack.Count > 0)
            {
                item = stack.Pop();
                return true;
            }

            item = default(T);
            return false;
        }
    }
}