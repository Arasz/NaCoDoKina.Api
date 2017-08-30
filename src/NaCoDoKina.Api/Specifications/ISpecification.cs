using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NaCoDoKina.Api.Specifications
{
    /// <summary>
    /// Contains query logic 
    /// </summary>
    /// <seealso cref="http://deviq.com/specification-pattern/"/>
    /// <typeparam name="T"> Type for which query is defined </typeparam>
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }

        void AddInclude(Expression<Func<T, object>> includeExpression);
    }
}