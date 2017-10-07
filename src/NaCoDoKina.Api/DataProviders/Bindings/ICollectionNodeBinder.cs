using AngleSharp.Dom;
using NaCoDoKina.Api.Infrastructure.Settings;
using System.Collections.Generic;

namespace NaCoDoKina.Api.DataProviders.Bindings
{
    /// <summary>
    /// Binds parent node content to collection of elements 
    /// </summary>
    /// <typeparam name="TBinded"></typeparam>
    public interface ICollectionNodeBinder<out TBinded>
    {
        /// <summary>
        /// Reads collection of elements from node and binds it to TBinded objects 
        /// </summary>
        IEnumerable<TBinded> Bind(IParentNode node, PropertySelector[] propertySelectors, string collectionSelector);
    }
}