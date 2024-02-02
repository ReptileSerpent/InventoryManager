using InventoryManager.Helpers;

namespace InventoryManager.DatabaseAccess.Interfaces
{
    internal interface IDatabaseController
    {
        public Result TryCreateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity;
        public Result TryReadEntityById<T>(uint id, out T entity) where T : class, Data.Interfaces.IEntity, new();
        public Result TryReadEntityByCode<T>(string code, out T entity) where T : class, Data.Interfaces.IEntityWithCode, new();
        public Result TryUpdateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity;
        public Result TryDeleteEntityById<T>(uint id) where T : class, Data.Interfaces.IEntity;
        public Result TryDeleteEntityByCode<T>(string code) where T : class, Data.Interfaces.IEntityWithCode;
    }
}
