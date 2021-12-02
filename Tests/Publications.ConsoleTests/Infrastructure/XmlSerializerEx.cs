using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Publications.ConsoleTests.Infrastructure;

internal class XmlSerializerEx
{
    public static XmlSerializer Create(Type type)
    {
        return XmlSerializersFactory.Default.Create(type);
    }
}

internal class XmlSerializersFactory
{
    //private static readonly object __SingletonSyncRoot = new();
    //private static XmlSerializersFactory __Factory;

    //public static XmlSerializersFactory Default
    //{
    //    get
    //    {
    //        if (__Factory != null) return __Factory;

    //        lock (__SingletonSyncRoot)
    //        {
    //            //if (__Factory != null) return __Factory;
    //            //__Factory = new();
    //            return __Factory ??= new();
    //        }
    //    }
    //}

    private static readonly Lazy<XmlSerializersFactory> __Factory = new(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static XmlSerializersFactory Default => __Factory.Value;


    #region Async singleton

    private static readonly Lazy<Task<XmlSerializersFactory>> __FactoryAsync = new(() => CreateFactoryAsync(), true);

    private static Task<XmlSerializersFactory> CreateFactoryAsync()
    {
        return Task.Run(async () =>
        {
            await Task.Delay(100);
            return new XmlSerializersFactory();
        });
    }

    public static XmlSerializersFactory DefaultAsync => __FactoryAsync.Value.Result;

    #endregion

    //private readonly Dictionary<Type, XmlSerializer> _Serializers = new();

    //public XmlSerializer Create(Type type)
    //{
    //    return _Serializers.TryGetValue(type, out var serializer)
    //        ? serializer
    //        : _Serializers[type] = new XmlSerializer(type);
    //}

    private XmlSerializersFactory() { }

    private readonly ConcurrentDictionary<Type, XmlSerializer> _Serializers = new();

    public XmlSerializer Create(Type type) => _Serializers.GetOrAdd(type, t => new(t));

    public void Clear() => _Serializers.Clear();
}
