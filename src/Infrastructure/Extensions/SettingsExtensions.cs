using Microsoft.Extensions.Configuration;
using System;

namespace Infrastructure.Extensions
{
    public static class SettingsExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration) where T : new()
        {
            var section = typeof(T).Name.Replace("Settings", string.Empty);
            var configurationValue = new T();
            configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }

        public static object GetSettings(this IConfiguration configuration, Type type)
        {
            var section = type.Name.Replace("Settings", string.Empty);
            var configurationValue = Activator.CreateInstance(type);
            configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }
    }
}