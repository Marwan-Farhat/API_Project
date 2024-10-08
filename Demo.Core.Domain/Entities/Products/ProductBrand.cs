namespace Demo.Core.Domain.Entities.Products
{
    public class ProductBrand: BaseAuditableEntity<int>
    {
        public required string Name { get; set; }

    }
}
