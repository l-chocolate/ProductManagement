namespace ProductManagement.API.DTOs
{
    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public double? UnitCost { get; set; }
    }
}
