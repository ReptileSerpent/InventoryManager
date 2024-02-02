using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class Product : Interfaces.IEntityWithCode
    {
        public uint Id { get; set; }

        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public virtual Category Category { get; set; } = null!;
        public string? Description { get; set; }
    }
}