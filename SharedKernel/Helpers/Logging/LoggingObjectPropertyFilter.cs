using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

namespace SharedKernel.Helpers.Logging;

public sealed class LoggingObjectPropertyFilter : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty prop = base.CreateProperty(member, memberSerialization);
        var propType = prop.PropertyType;
        if (!IsConcreteType(propType) || IsBlob(propType) || HasIgnoreAttribute(member))
            prop.Ignored = true;

        return prop;
    }

    public static bool IsConcreteType(Type propType) => !propType.IsInterface && !propType.IsAbstract;

    public static bool IsBlob(Type propType) => propType == typeof(byte[]) || propType == typeof(Stream);

    public static bool HasIgnoreAttribute(MemberInfo member) => Attribute.IsDefined(member, typeof(NoLogging));
}
