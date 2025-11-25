using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Domain.Entities
{
    [Table("ProductEvent")]
    public class ProductEvent : Entity
    {
        public int ProductId { get; set; }
        public required string Payload { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
