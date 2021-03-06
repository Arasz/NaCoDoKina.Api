﻿using System.Threading.Tasks;
using ApplicationCore.Results;

namespace Infrastructure.DataProviders.Bindings
{
    /// <summary>
    /// Binds html and binds data to object of given class 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IWebPageBinder<in TEntity>
    {
        /// <summary>
        /// Binds html under url to given object 
        /// </summary>
        /// <param name="binded"> Object to fill </param>
        /// <param name="url"> Web page url </param>
        Task<Result> BindAsync(TEntity binded, string url);
    }
}