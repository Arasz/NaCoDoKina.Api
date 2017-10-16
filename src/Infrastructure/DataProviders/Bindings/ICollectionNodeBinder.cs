using System.Collections.Generic;
using AngleSharp.Dom;
using Infrastructure.Settings;
using Infrastructure.Settings.Common;

namespace Infrastructure.DataProviders.Bindings
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