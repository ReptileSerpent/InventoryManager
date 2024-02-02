using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class Warehouse : Interfaces.IEntityWithCode
    {
        public uint Id { get; set; }

        public string Code { get; set; } = null!;
        public virtual Location Location { get; set; } = null!;
    }
}