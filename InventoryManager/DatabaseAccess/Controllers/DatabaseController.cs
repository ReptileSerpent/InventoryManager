using InventoryManager.Data;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;

namespace InventoryManager.DatabaseAccess.Controllers
{
    internal class DatabaseController : IDatabaseController
    {
        public DatabaseController()
        {
            dbContext = new InventoryContext();
        }

        private InventoryContext dbContext;

        public Result TryCreateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(dbContext);
                var createdEntity = repository.Create(entity);
                dbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = e.ToString()
                };
            }
        }

        public Result TryReadEntityById<T>(uint id, out T entity) where T : class, Data.Interfaces.IEntity, new()
        {
            entity = new T();

            try
            {
                var repository = new Data.Repositories.Repository<T>(dbContext);
                var readEntity = repository.ReadById(id);
                if (readEntity == null)
                    throw new InvalidOperationException("The read operation unexpectedly returned a null entity.");
                entity = readEntity;
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = e.ToString()
                };
            }
        }

        public Result TryReadEntityByCode<T>(string code, out T entity) where T : class, Data.Interfaces.IEntityWithCode, new()
        {
            entity = new T();

            try
            {
                var repository = new Data.Repositories.EntityWithCodeRepository<T>(dbContext);
                var readEntity = repository.ReadByCode(code);
                if (readEntity == null)
                    throw new InvalidOperationException("The read operation unexpectedly returned a null entity.");
                entity = readEntity;
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = e.ToString()
                };
            }
        }

        public Result TryUpdateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(dbContext);
                var updatedEntity = repository.Update(entity);
                dbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = e.ToString()
                };
            }
        }

        public Result TryDeleteEntityById<T>(uint id) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(dbContext);
                repository.DeleteById(id);
                dbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = e.ToString()
                };
            }
        }

        public Result TryDeleteEntityByCode<T>(string code) where T : class, Data.Interfaces.IEntityWithCode
        {
            try
            {
                var repository = new Data.Repositories.EntityWithCodeRepository<T>(dbContext);
                repository.DeleteByCode(code);
                dbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = e.ToString()
                };
            }
        }
    }
}
