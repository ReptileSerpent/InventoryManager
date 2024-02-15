using InventoryManager.Data;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;
using Microsoft.Extensions.Logging;

namespace InventoryManager.DatabaseAccess.Controllers
{
    internal class DatabaseController : IDatabaseController
    {
        public DatabaseController(ILogger logger)
        {
            Logger = logger;
            dbContext = new InventoryContext();
        }

        private ILogger Logger { get; }
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
                Logger.LogError(e, "Failed to create entity of type {0}", typeof(T).Name);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to create entity: {e.Message} See the log file for the detailed exception and stack trace."
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
                Logger.LogError(e, "Failed to read entity of type {0} by id {1}", typeof(T).Name, id);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to read entity: {e.Message} See the log file for the detailed exception and stack trace."
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
                Logger.LogError(e, "Failed to create entity of type {0} by code {1}", typeof(T).Name, code);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to read entity: {e.Message} See the log file for the detailed exception and stack trace."
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
                Logger.LogError(e, $"Failed to update entity of type {0}", typeof(T).Name);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to update entity: {e.Message} See the log file for the detailed exception and stack trace."
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
                Logger.LogError(e, $"Failed to delete entity of type {0} by id {1}", typeof(T).Name, id);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to delete entity: {e.Message} See the log file for the detailed exception and stack trace."
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
                Logger.LogError(e, $"Failed to delete entity of type {0} by code {1}", typeof(T).Name, code);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to delete entity: {e.Message}. See the log file for the detailed exception and stack trace."
                };
            }
        }
    }
}
