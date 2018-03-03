using System.IO;
using GroBuf.DataMembersExtracters;

namespace MetricsCollector
{
    public static class Serializer
    {
        public static void Serialize<T>(T obj, MemoryStream stream)
        {
            using (var grobuf = GetSerializer())
            {
                var index = (int) stream.Position;
                grobuf.Value.Serialize(obj, stream.GetBuffer(), ref index);
                stream.SetLength(index);
            }
        }
        
        public static int GetEstimatedSerializedSize<T>(T obj)
        {
            using (var grobuf = GetSerializer())
            {
                return grobuf.Value.GetSize(obj);
            }
        }

        private static Disposable<GroBuf.Serializer> GetSerializer()
        {
            return serializer;
        }
        
        private static GroBuf.Serializer serializer = new GroBuf.Serializer(new AllFieldsExtractor());
    }
}