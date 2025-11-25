using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Domain.Entities
{
    [Table("Product")]
    public class Product : Entity
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public double UnitCost { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
