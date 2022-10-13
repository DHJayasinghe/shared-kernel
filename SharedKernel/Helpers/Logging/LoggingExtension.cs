using Newtonsoft.Json;

namespace SharedKernel.Helpers.Logging;

public static class LoggingExtension
{
    public static T DeepCloneWithLoggingFilters<T>(this T objSource)
    {
        var serialized = JsonConvert.SerializeObject(objSource, new JsonSerializerSettings() { ContractResolver = new LoggingObjectPropertyFilter() });
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}