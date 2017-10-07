using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaCoDoKina.Api.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly Dictionary<Type, PropertyInfo[]> PropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> SinglePropertiesCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        public static bool HasProperty(this object obj, string propertyName)
        {
            var type = obj.GetType();

            LoadPropertiesToCache(type, propertyName);

            return PropertiesCache[type]
                .Any(info => info.Name == propertyName);
        }

        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            var type = obj.GetType();

            LoadPropertiesToCache(type, propertyName);

            var property = GetPropertyFromCache(type, propertyName);

            property.SetValue(obj, value);
        }

        public static TValue GetPropertyValue<TValue>(this object obj, string propertyName)
        {
            var type = obj.GetType();

            LoadPropertiesToCache(type, propertyName);

            var property = GetPropertyFromCache(type, propertyName);

            var value = property?.GetValue(obj);

            if (value is TValue result)
                return result;

            return default(TValue);
        }

        public static PropertyInfo GetProperty(this object obj, string propertyName)
        {
            var type = obj.GetType();

            LoadPropertiesToCache(type, propertyName);

            return GetPropertyFromCache(type, propertyName);
        }

        private static PropertyInfo GetPropertyFromCache(Type propertyType, string propertyName)
        {
            return SinglePropertiesCache[propertyType][propertyName];
        }

        private static void LoadPropertiesToCache(Type type, string propertyName)
        {
            if (!PropertiesCache.ContainsKey(type))
            {
                PropertiesCache[type] = type.GetProperties();
                SinglePropertiesCache[type] = new Dictionary<string, PropertyInfo>();
            }

            if (!SinglePropertiesCache[type].ContainsKey(propertyName))
            {
                var property = PropertiesCache[type]
                    .FirstOrDefault(info => info.Name == propertyName);

                SinglePropertiesCache[type][propertyName] = property;
            }
        }
    }
}