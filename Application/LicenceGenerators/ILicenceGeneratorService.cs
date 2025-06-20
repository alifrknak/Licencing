
using Application.Model;
using Domain;

namespace Business.Services.LicenceGenerators;

public interface ILicenceGeneratorService
{
    void GenerateLicence(IEnumerable<FeatureModel> features, User user);

    bool Validate(string licenceText);

}
