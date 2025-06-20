namespace Application.Model;

public class ProductModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; }

}
