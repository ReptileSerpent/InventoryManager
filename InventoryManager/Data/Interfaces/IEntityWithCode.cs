namespace InventoryManager.Data.Interfaces
{
    internal interface IEntityWithCode : IEntity
    {
        public string Code { get; set; }
    }
}
