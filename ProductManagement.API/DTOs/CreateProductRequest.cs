namespace ProductManagement.API.DTOs
{
    public class CreateProductRequest
    {
        public required string Name { get; set; }
        public required string CategoryName { get; set; }
        public double UnitCost { get; set; }
    }
}
