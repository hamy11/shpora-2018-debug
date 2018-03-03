using System;
using System.IO;

namespace MetricsCollector
{
    public class MetricsStreamWriter
    {
        private readonly Stream targetStream;

        public MetricsStreamWriter(Stream targetStream)
        {
            this.targetStream = targetStream;
        }

        public void Save(MetricsCollection collection)
        {
            var serializedSize = Serializer.GetEstimatedSerializedSize(collection);
            using (var stream = StreamPool.Get(serializedSize))
            {
                Serializer.Serialize(collection, stream);
                targetStream.Write(BitConverter.GetBytes((int) stream.Value.Length), 0, sizeof(int));
                stream.Value.CopyToAsync(targetStream);
            }
        }
    }
}