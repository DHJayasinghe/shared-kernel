using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.EventBus;

internal static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }

    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
    {
        List<T> objects = new List<T>();
        foreach (Type type in typeof(EventBusCore).Assembly.GetTypes()
            .Where(myType => myType.IsClass
                && !myType.IsAbstract
                && typeof(T).IsAssignableFrom(myType)))
        {
            objects.Add((T)Activator.CreateInstance(type, constructorArgs));
        }
        return objects;
    }

    public static TValue GetAttributeValue<TAttribute, TValue>(
        this Type type,
        Func<TAttribute, TValue> valueSelector
    ) where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att
            ? valueSelector(att)
            : default;
}
