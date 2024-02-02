namespace InventoryManager.Data.Repositories
{
    internal class EntityWithCodeRepository<T> : Repository<T> where T : class, Interfaces.IEntityWithCode
    {
        public EntityWithCodeRepository(InventoryContext dbContext) : base(dbContext) { }

        public T? FindByCode(string code)
        {
            var dataSet = dbContext.Set<T>();
            return dataSet.FirstOrDefault(row => row.Code == code);
        }

        public T? ReadByCode(string code)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Code == code);
            return searchResult;
        }

        public T? DeleteByCode(string code)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Code == code);
            if (searchResult != null)
            {
                dataSet.Remove(searchResult);
                dbContext.SaveChanges();
            }
            return searchResult;
        }
    }
}
