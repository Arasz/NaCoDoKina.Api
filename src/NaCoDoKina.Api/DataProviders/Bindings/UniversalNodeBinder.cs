using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaCoDoKina.Api.DataProviders.Bindings
{
    public class UniversalDocumentBinder<TBinded> : IDocumentBinder<TBinded>
    {
        private readonly ILogger<UniversalDocumentBinder<TBinded>> _logger;
        private readonly Dictionary<string, PropertyInfo> _properties;

        public UniversalDocumentBinder(ILogger<UniversalDocumentBinder<TBinded>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _properties = typeof(TBinded)
                .GetProperties()
                .ToDictionary(info => info.Name, info => info);
        }

        public void Bind(TBinded binded, IParentNode parentElement, PropertySelector[] propertySelectors)
        {
            using (_logger.BeginScope(nameof(Bind)))
            {
                foreach (var propertySelector in propertySelectors)
                {
                    _logger.LogDebug("Read property {property} for type {type}", propertySelector.PropertyName, typeof(TBinded));

                    var property = _properties[propertySelector.PropertyName];

                    _logger.LogDebug("Select element with selector {selector}", propertySelector.Selector);

                    var element = parentElement.QuerySelector(propertySelector.Selector);

                    if (element is null)
                    {
                        _logger.LogWarning("Wrong CSS selector {selector} defined in property selector {@propertySelector} for binded element {@binded} ", propertySelector.Selector, propertySelector, binded);
                        return;
                    }

                    var value = element.TextContent;

                    if (propertySelector.FromAttribute && !propertySelector.Attribute.IsNullOrEmpty())
                        value = element.GetAttribute(propertySelector.Attribute);

                    _logger.LogDebug("Read element value value {value}. Details {@selector}", value, propertySelector);

                    switch (property.PropertyType.Name)
                    {
                        case nameof(DateTime):
                            _logger.LogDebug("Set {type} property with value {value}", property.PropertyType.Name, value);
                            if (DateTime.TryParse(value, out var parsedDate))
                            {
                                property.SetValue(binded, parsedDate);
                            }
                            break;

                        case nameof(String):
                            _logger.LogDebug("Set {type} property with value {value}", property.PropertyType.Name, value);
                            property.SetValue(binded, value);
                            break;

                        default:
                            _logger.LogError("Unsupported property type {type}", property.PropertyType.Name);
                            break;
                    }
                }
            }
        }
    }
}