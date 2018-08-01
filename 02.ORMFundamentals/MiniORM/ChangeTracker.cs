using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
    internal class ChangeTracker<T>
        where T : class, new()
    {
        private readonly List<T> allEntities;
        private readonly List<T> added;
        private readonly List<T> removed;


        public ChangeTracker(IEnumerable<T> entities)
        {
            this.added = new List<T>();
            this.removed = new List<T>();

            this.allEntities = CloneEntities(entities);
        }

        public IReadOnlyCollection<T> AllEntities => this.allEntities.AsReadOnly();

        public IReadOnlyCollection<T> Added => this.added.AsReadOnly();

        public IReadOnlyCollection<T> Removed => this.removed.AsReadOnly();

        private List<T> CloneEntities(IEnumerable<T> entities)
        {
            List<T> clonedEntities = new List<T>();

            PropertyInfo[] propertiesToClone = typeof(T)
                .GetProperties()
                .Where(propInfo => DbContext.AllowedSqlTypes.Contains(propInfo.PropertyType))
                .ToArray();

            foreach (var entity in entities)
            {
                T clonedEntity = Activator.CreateInstance<T>();

                foreach (var property in propertiesToClone)
                {
                    object value = property.GetValue(entity);
                    property.SetValue(clonedEntity, value);
                }

                clonedEntities.Add(clonedEntity);
            }

            return clonedEntities;
        }

        public void Add(T item) => this.added.Add(item);

        public void Remove(T item) => this.removed.Add(item);

        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            List<T> modifiedEntities = new List<T>();

            var primaryKeys = typeof(T)
                .GetProperties()
                .Where(propInfo => propInfo.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (var proxyEntity in this.AllEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity).ToArray();

                var entity = dbSet.Entities
                    .Single(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

                var isModified = IsModified(proxyEntity, entity);
                if (isModified)
                {
                    modifiedEntities.Add(entity);
                }
            }

            return modifiedEntities;
        }

        private bool IsModified(T entity, T proxyEntity)
        {
            var monitoredProperties = typeof(T)
                .GetProperties()
                .Where(propInfo => DbContext.AllowedSqlTypes.Contains(propInfo.PropertyType));


            var modifiedProperties = monitoredProperties
                .Where(propInfo => !Equals(propInfo.GetValue(entity), propInfo.GetValue(proxyEntity)))
                .ToArray();

            var isModified = modifiedProperties.Any();

            return isModified;
        }

        private IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T entity)
        {
            IEnumerable<object> primaryKeysValues = primaryKeys.Select(pk => pk.GetValue(entity));

            return primaryKeysValues;
        }


    }
}