using Confluent.Kafka;
using System;
using System.Text;

namespace Ckr.Serialization
{
    public class StringDeserializer : IDeserializer<string>
    {
        public static readonly IDeserializer<string> Instance = new StringDeserializer();

        public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.UTF8.GetString(data);
            }
        }
    }
}
