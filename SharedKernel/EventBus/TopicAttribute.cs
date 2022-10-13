using System;

namespace SharedKernel.EventBus
{
    public sealed class TopicAttribute : Attribute
    {
        public string Name { get; }
        public TopicAttribute(string name) => Name = name;
    }
}
