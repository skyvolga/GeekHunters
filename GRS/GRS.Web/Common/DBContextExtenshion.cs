using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GRS.Web.Common
{
    public static class DBContextExtenshion
    {
        public static void UpdateEntities<TEntity, TKey>(
            this DbContext context,
            List<TEntity> existedEntities,
            List<TEntity> entities,
            Func<TEntity, TKey> key,
            Action<TEntity, TEntity> update = null
        )
            where TEntity : class
        {
            var existedEntityDictionary = existedEntities.ToDictionary(key);
            var entityDictionary = entities.ToDictionary(key);

            var deletedEntityKeys = existedEntityDictionary.Keys
                .Except(entityDictionary.Keys);

            var addedEntityKeys = entityDictionary.Keys
                .Except(existedEntityDictionary.Keys);
            var updatedEntityKeys = entityDictionary.Keys
                .Intersect(existedEntityDictionary.Keys);

            if (update != null)
            {
                foreach (var updatedEntityKey in updatedEntityKeys)
                {
                    update(existedEntityDictionary[updatedEntityKey],
                           entityDictionary[updatedEntityKey]);
                }
            }

            if (deletedEntityKeys.Any())
            {
                context.RemoveRange(deletedEntityKeys
                    .Select(x => existedEntityDictionary[x]));
            }

            if (addedEntityKeys.Any())
            {
                context.AddRange(addedEntityKeys
                    .Select(x => entityDictionary[x]));
            }
        }

        public static void AddOrUpdate(this DbContext context, object entity)
        {
            var entry = context.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    context.Add(entity);
                    break;
                case EntityState.Modified:
                    context.Update(entity);
                    break;
                case EntityState.Added:
                    context.Add(entity);
                    break;
                case EntityState.Unchanged:
                    //item already in db no need to do anything  
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
