namespace InventoryManager.Data.Entities
{
    public class InventoryEntry : Interfaces.IEntity
    {
        public uint Id { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
        public uint Count { get; set; }
    }
}