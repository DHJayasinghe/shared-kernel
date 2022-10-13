using System;

namespace SharedKernel.EventBus;

public sealed class EventBusMessageWrapper
{
    public string Assembly { get; set; }
    public string TypeName { get; set; }
    public object Payload { get; set; }

    private EventBusMessageWrapper() { }
    public EventBusMessageWrapper(object payload) : this()
    {
        Payload = payload;
        Assembly = payload.GetType().AssemblyQualifiedName;
        TypeName = payload.GetType().Name;
    }

    public Type GetType(Type callerContext = null) =>
        callerContext == null
           ? Type.GetType(Assembly)
           : callerContext.Assembly.GetType($"{callerContext.Namespace}.{TypeName}");
}