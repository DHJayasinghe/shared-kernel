using Newtonsoft.Json;
using SharedKernel.Interfaces.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.EventBus;

public class EventBusCore
{
    protected EventBusCore() { }

    public static object MessageDeserializer(byte[] message)
    {
        var messageBody = System.Text.Encoding.UTF8.GetString(message);
        var unwrappedMessage = JsonConvert.DeserializeObject<EventBusMessageWrapper>(messageBody);
        return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(unwrappedMessage.Payload), Type.GetType(unwrappedMessage.Assembly), new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }

    public static object MessageDeserializer(string message)
    {
        var unwrappedMessage = JsonConvert.DeserializeObject<EventBusMessageWrapper>(message);
        return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(unwrappedMessage.Payload), Type.GetType(unwrappedMessage.Assembly), new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }

    public static object MessageDeserializer(Type callerContext, string message)
    {
        var unwrappedMessage = JsonConvert.DeserializeObject<EventBusMessageWrapper>(message);
        var type = unwrappedMessage.GetType(callerContext);
        if (type is null) return null;

        var notification = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(unwrappedMessage.Payload), type, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        return notification;
    }

    public static EventBusMessageWrapper UnwrappedMessageDeserializer(string message)
    {
        return JsonConvert.DeserializeObject<EventBusMessageWrapper>(message);

    }

    public static object MessageDeserializer(Type callerContext, EventBusMessageWrapper unwrappedMessage)
    {
        var type = unwrappedMessage.GetType(callerContext);
        if (type is null) return null;

        var notification = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(unwrappedMessage.Payload), type, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        return notification;
    }
    /// <summary>
    /// Returns inherited topics
    /// </summary>
    public static IEnumerable<IEventBusTopic> GetTopics() => ReflectiveEnumerator.GetEnumerableOfType<IEventBusTopic>().ToList();

    /// <summary>
    /// Returns inherited queues
    /// </summary>
    public static IEnumerable<IEventBusQueue> GetQueues() => ReflectiveEnumerator.GetEnumerableOfType<IEventBusQueue>().ToList();

    /// <summary>
    /// Returns Subscriptions related to a Topic
    /// </summary>
    public static IEnumerable<string> GetSubscriptions(string topicName) =>
            ReflectiveEnumerator.GetEnumerableOfType<IEventBusTopic>()
                .FirstOrDefault(d => d.GetType().Name == topicName)
                ?.GetType().GetAttributeValue((SubscriptionAttribute subs) => subs.Names);

    /// <summary>
    /// Extract just the 2 parts of the assembly name.
    /// Ex: BnA.PM.Maintenance.Domain.Events.JobCompletedEvent -> BnA.PM
    /// </summary>
    public static string GetAppName(string assemblyName) => string.Join(".", assemblyName.Split(".").Take(3));
}
