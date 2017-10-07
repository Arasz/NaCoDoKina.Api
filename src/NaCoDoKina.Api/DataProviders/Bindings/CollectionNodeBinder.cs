using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NaCoDoKina.Api.DataProviders.Bindings
{
    public class CollectionNodeBinder<TBinded> : ICollectionNodeBinder<TBinded>
        where TBinded : new()
    {
        private readonly INodeBinder<TBinded> _nodeBinder;
        private readonly ILogger<CollectionNodeBinder<TBinded>> _logger;

        public CollectionNodeBinder(INodeBinder<TBinded> nodeBinder, ILogger<CollectionNodeBinder<TBinded>> logger)
        {
            _nodeBinder = nodeBinder ?? throw new ArgumentNullException(nameof(nodeBinder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private TBinded BindSelectedElement(IElement element, PropertySelector[] propertySelectors)
        {
            _logger.LogDebug("Bind selected element with {elementId}, {tagName}, {@attributes} to type {type}",
                element.Id, element.TagName, element.Attributes, typeof(TBinded));

            var binded = new TBinded();
            _nodeBinder.Bind(binded, element, propertySelectors);
            return binded;
        }

        public IEnumerable<TBinded> Bind(IParentNode node, PropertySelector[] propertySelectors, string collectionSelector)
        {
            using (_logger.BeginScope(nameof(Bind)))
            {
                _logger.LogDebug("Select multiple elements with selector {selector}", collectionSelector);

                var selectedElements = node.QuerySelectorAll(collectionSelector);

                _logger.LogDebug("Selected {elementsCount}", selectedElements.Length);

                return selectedElements
                    .Select(element => BindSelectedElement(element, propertySelectors))
                    .ToArray();
            }
        }
    }
}