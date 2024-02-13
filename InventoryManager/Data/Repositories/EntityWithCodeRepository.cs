namespace InventoryManager.Data.Repositories
{
    internal class EntityWithCodeRepository<T> : Repository<T> where T : class, Interfaces.IEntityWithCode
    {
        public EntityWithCodeRepository(InventoryContext dbContext) : base(dbContext) { }

        /// <returns>The entity if one with the given code exists; otherwise, null.</returns>
        public T? ReadByCode(string code)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Code == code);
            return searchResult;
        }

        /// <returns>The entity if one with the given code exists prior to deletion; otherwise, null.</returns>
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
