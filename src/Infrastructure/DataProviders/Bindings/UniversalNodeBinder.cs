using AngleSharp.Dom;
using ApplicationCore.Results;
using Infrastructure.Extensions;
using Infrastructure.Settings.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Infrastructure.DataProviders.Bindings
{
    public class UniversalNodeBinder<TBinded> : INodeBinder<TBinded>
    {
        private readonly ILogger<UniversalNodeBinder<TBinded>> _logger;

        private BindingErrors<TBinded> _bindingErrors;

        public UniversalNodeBinder(ILogger<UniversalNodeBinder<TBinded>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Result Bind(TBinded binded, IParentNode node, PropertySelector[] propertySelectors)
        {
            _bindingErrors = new BindingErrors<TBinded>(propertySelectors);

            using (_logger.BeginScope(nameof(Bind)))
            {
                foreach (var propertySelector in propertySelectors)
                {
                    _logger.LogDebug("Read property {PropertyName} for type {Type}", propertySelector.PropertyName, typeof(TBinded));

                    var (propertyName, selector) = propertySelector;

                    if (!binded.HasProperty(propertyName))
                    {
                        _logger.LogDebug("Binded {Type} do not has property {PropertyName}", nameof(TBinded), propertyName);
                        _bindingErrors.AddError(propertySelector.PropertyName, $"Binded type {nameof(TBinded)} do not has property {propertyName}");
                        continue;
                    }

                    _logger.LogDebug("Select property {PropertyName} with selector {selector} from page {Url}", propertyName, selector, node.FirstElementChild?.BaseUri);

                    var element = node.QuerySelector(selector);

                    if (element is null)
                    {
                        _bindingErrors.AddError(propertyName, $"Element specified by selector {selector} can not be found");
                        continue;
                    }

                    var value = GetValueFromElement(element, propertySelector);

                    if (value is null)
                    {
                        _logger.LogWarning("Element {Tag} value selected by {@PropertySelector} from page {Url} is null", element.TagName, propertySelector, element.BaseUri);
                        continue;
                    }

                    _logger.LogDebug("{Tag} element value {Value} selected by {@PropertySelector} from page {Url}", element.TagName, value, propertySelector, element.BaseUri);

                    ParsePropertyValueToType(binded, propertyName, value);
                }

                if (_bindingErrors.HasErrors())
                    return Result.Failure(_bindingErrors.ToString());

                return Result.Success();
            }
        }

        private static string GetValueFromElement(IElement element, PropertySelector propertySelector)
        {
            var value = element.TextContent;
            if (propertySelector.FromAttribute && !propertySelector.Attribute.IsNullOrEmpty())
                value = element.GetAttribute(propertySelector.Attribute);
            return value;
        }

        private void ParsePropertyValueToType(TBinded binded, string propertyName, string value)
        {
            var property = binded.GetProperty(propertyName);

            switch (property.PropertyType.Name)
            {
                case nameof(DateTime):
                    if (DateTime.TryParse(value, out var parsedDate))
                    {
                        binded.SetPropertyValue(propertyName, parsedDate);
                    }
                    break;

                case nameof(String):
                    binded.SetPropertyValue(propertyName, value);
                    break;

                default:
                    _logger.LogError("Unsupported property type {type}", property.PropertyType.Name);
                    _bindingErrors.AddError(property.Name, "Unsupported property type");
                    break;
            }
        }
    }
}