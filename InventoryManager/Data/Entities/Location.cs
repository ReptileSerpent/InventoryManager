using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class Location : Interfaces.IEntityWithCode
    {
        public uint Id { get; set; }

        public string Code { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
    }
}