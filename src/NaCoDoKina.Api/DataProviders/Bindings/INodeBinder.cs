using AngleSharp.Dom;
using NaCoDoKina.Api.Infrastructure.Settings;

namespace NaCoDoKina.Api.DataProviders.Bindings
{
    /// <summary>
    /// Binds node to TBinded type element 
    /// </summary>
    /// <typeparam name="TBinded"></typeparam>
    public interface INodeBinder<in TBinded>
    {
        /// <summary>
        /// Binds html node to object using provided property selectors 
        /// </summary>
        /// <param name="binded"> Mapped object </param>
        /// <param name="node"> Html parent element with data, data source </param>
        /// <param name="propertySelectors"> Property to selector bindings </param>
        void Bind(TBinded binded, IParentNode node, PropertySelector[] propertySelectors);
    }
}