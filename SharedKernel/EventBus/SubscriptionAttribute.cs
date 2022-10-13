using System;
using System.Collections.Generic;

namespace SharedKernel.EventBus;

public sealed class SubscriptionAttribute : Attribute
{
    public IEnumerable<string> Names { get; }

    public SubscriptionAttribute(params string[] names) => Names = names;
}
