using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Try attach entity to current session. If entity is already attached, returns false 
        /// </summary>
        /// <typeparam name="TEntity"> Entity type </typeparam>
        /// <param name="context"> DbContext </param>
        /// <param name="entity"> Attached entity </param>
        /// <returns> False if entity was already attached </returns>
        public static bool TryAttach<TEntity>(this DbContext context, TEntity entity)
            where TEntity : Entity
        {
            var isAttached = context.Set<TEntity>().Local.Any(e => e.Id == entity.Id);

            if (!isAttached)
                context.Attach(entity);

            return !isAttached;
        }
    }
}