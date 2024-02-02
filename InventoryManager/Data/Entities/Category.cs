using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class Category : Interfaces.IEntityWithCode
    {
        public uint Id { get; set; }

        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}