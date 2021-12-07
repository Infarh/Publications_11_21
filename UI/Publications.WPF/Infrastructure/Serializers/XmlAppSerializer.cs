using System.IO;
using System.Xml.Serialization;

namespace Publications.WPF.Infrastructure.Serializers;

public class XmlAppSerializer<T> : AppSerializer<T>
{
    private readonly XmlSerializer _Serializer;

    public XmlAppSerializer(XmlSerializer Serializer) => _Serializer = Serializer;

    public override void Serialize(T value, Stream stream)
    {
        _Serializer.Serialize(stream, value);
    }

    public override T? Deserialize(Stream stream)
    {
        return (T?)_Serializer.Deserialize(stream);
    }
}