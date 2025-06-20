namespace Domain;

public class UserLicense : BaseEntity
{

    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime ExpirationDate { get; set; }

    public string LicenseTextFormat { get; set; }

    public User User { get; set; }
    public Product Product { get; set; }

}