﻿namespace InventoryManager.Data.Repositories
{
    internal class Repository<T> where T : class, Interfaces.IEntity
    {
        protected readonly InventoryContext dbContext;

        public Repository(InventoryContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public T? Create(T entity)
        {
            if (entity != null)
            {
                var dataSet = dbContext.Set<T>();
                dataSet.Add(entity);
                dbContext.SaveChanges();
            }
            return entity;
        }

        public T? ReadById(uint id)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Id == id);
            return searchResult;
        }

        public T? Update(T entity)
        {
            if (entity != null)
            {
                var dataSet = dbContext.Set<T>();
                dataSet.Update(entity);
                dbContext.SaveChanges();
            }
            return entity;
        }

        public T? DeleteById(uint id)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Id == id);
            if (searchResult != null)
            {
                dataSet.Remove(searchResult);
                dbContext.SaveChanges();
            }
            return searchResult;
        }

        public T? FindById(uint id)
        {
            var dataSet = dbContext.Set<T>();
            // Supposedly Find is faster than SingleOrDefault
            return dataSet.Find(typeof(T), id);
        }
    }
}