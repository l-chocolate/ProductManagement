namespace ProductManagement.API.DTOs
{
    public class ProductResponse
    {
        public required int ProductId { get; set; }
        public required string Name { get; set; }
        public required string CategoryName { get; set; }
        public double UnitCost { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
