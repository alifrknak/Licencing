using System.ComponentModel.DataAnnotations;

namespace Application.Model;

public class CreateLicenseModel
{
    [Required]
    public Guid FeatureId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public DateTime ExpireDate { get; set; } = DateTime.Now;

    [Required]
    public int LicenseType { get; set; } // Assuming LicenseType is an integer enum

}