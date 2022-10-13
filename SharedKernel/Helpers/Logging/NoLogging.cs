using System;

namespace SharedKernel.Helpers.Logging;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NoLogging : Attribute
{
}