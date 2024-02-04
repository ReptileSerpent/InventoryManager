namespace InventoryManager.Data.Interfaces
{
    public interface IEntityWithCode : IEntity
    {
        public string Code { get; set; }
    }
}
