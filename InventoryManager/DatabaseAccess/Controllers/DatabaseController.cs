using InventoryManager.Data;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;
using Microsoft.Extensions.Logging;

namespace InventoryManager.DatabaseAccess.Controllers
{
    internal class DatabaseController : IDatabaseController
    {
        public DatabaseController(ILogger logger, InventoryContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        private ILogger Logger { get; }
        private InventoryContext DbContext { get; set; }

        public Result TryCreateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var createdEntity = repository.Create(entity);
                DbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to create entity of type {EntityType}", typeof(T).Name);
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
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var readEntity = repository.ReadById(id);
                if (readEntity == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = $"Couldn't find entity of type {typeof(T).Name} with id {id}."
                    };

                entity = readEntity;
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to read entity of type {EntityType} by id {Id}", typeof(T).Name, id);
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
                var repository = new Data.Repositories.EntityWithCodeRepository<T>(DbContext);
                var readEntity = repository.ReadByCode(code);
                if (readEntity == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = $"Couldn't find entity of type {typeof(T).Name} with code {code}."
                    };

                entity = readEntity;
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to create entity of type {EntityType} by code {Code}", typeof(T).Name, code);
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
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var updatedEntity = repository.Update(entity);
                DbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to update entity of type {EntityType}", typeof(T).Name);
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
                var repository = new Data.Repositories.Repository<T>(DbContext);
                repository.DeleteById(id);
                DbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to delete entity of type {EntityType} by id {Id}", typeof(T).Name, id);
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
                var repository = new Data.Repositories.EntityWithCodeRepository<T>(DbContext);
                repository.DeleteByCode(code);
                DbContext = new InventoryContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to delete entity of type {EntityType} by code {Code}", typeof(T).Name, code);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to delete entity: {e.Message}. See the log file for the detailed exception and stack trace."
                };
            }
        }
    }
}
