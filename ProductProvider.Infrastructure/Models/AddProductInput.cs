
namespace ProductProvider.Infrastructure.Models;

public class AddProductInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public string CategoryName { get; set; }
    public List<VariantInput> Variants { get; set; }
    public List<ReviewInput> Reviews { get; set; }
}

public class VariantInput
{
    public string Size { get; set; }
    public string Color { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}

public class ReviewInput
{
    public string ClientName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
