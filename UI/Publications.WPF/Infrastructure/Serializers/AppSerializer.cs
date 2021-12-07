using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publications.WPF.Infrastructure.Serializers;

public abstract class AppSerializer<T>
{
    public abstract void Serialize(T value, Stream stream);

    public abstract T? Deserialize(Stream stream);
}