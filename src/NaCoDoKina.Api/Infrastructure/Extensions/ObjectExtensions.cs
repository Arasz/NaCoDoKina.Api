using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaCoDoKina.Api.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly Dictionary<Type, PropertyInfo[]> PropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        public static bool HasProperty(this object obj, string propertyName)
        {
            var type = obj.GetType();

            if (!PropertiesCache.ContainsKey(type))
                PropertiesCache[type] = type.GetProperties();

            return PropertiesCache[type]
                .Any(info => info.Name == propertyName);
        }

        public static TValue PropertyValue<TValue>(this object obj, string propertyName)
        {
            var type = obj.GetType();

            if (!PropertiesCache.ContainsKey(type))
                PropertiesCache[type] = type.GetProperties();

            var property = PropertiesCache[type]
                .Where(info => info.PropertyType == typeof(TValue))
                .FirstOrDefault(info => info.Name == propertyName);

            var value = property?.GetValue(obj);

            if (value is TValue result)
                return result;

            return default(TValue);
        }
    }
}