using SharedKernel.Entity;
using System.ComponentModel;
using System.Linq;

namespace SharedKernel;

/// <summary>
/// Integration events are used to communicate between bounded contexts and/or applications.
/// They are often mapped from domain events in the notifying system 
/// and sometimes to domain events in the consuming system
/// </summary>
public class BaseIntegrationEvent : BaseDomainEvent
{
    public new string Origin { get; set; }

    public string GetName()
    {
        var descriptions = (DescriptionAttribute[])
           GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);

        return descriptions.Length != 0 ? descriptions[0].Description : GetType().Name;
    }

    public BaseIntegrationEvent()
    {
        Origin = string.Join(".", GetType().AssemblyQualifiedName.Split(".").Take(3));
    }
}