using Application.Model;
using Domain;
using Standard.Licensing;
using Standard.Licensing.Security.Cryptography;
using Standard.Licensing.Validation;
using System.Text;
using System.Xml;

namespace Business.Services.LicenceGenerators;

public class LicenceGeneratorService : ILicenceGeneratorService
{
    /// <summary>
    ///  Verilen bilgilere göre lisans üretir ve  Licence parametresinin LicenceText özelliğine atar.
    /// </summary>
    public void GenerateLicence(IEnumerable<FeatureModel> features, User user)
    {
        Dictionary<string, string> feature = features.ToDictionary(feature => feature.Name, feature => feature.Detail);
        Dictionary<string, string> additionals = new Dictionary<string, string>
        {
            {"GeneratedDate", DateTime.Now.ToString() }
        };

        string passPhrase = "YourPassPhrase123._";

        KeyGenerator keyGenerator = KeyGenerator.Create();
        KeyPair keyPair = keyGenerator.GenerateKeyPair();
        string privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
        string publicKey = keyPair.ToPublicKeyString();

        License generatedLicense = License.New()
            .WithUniqueIdentifier(Guid.NewGuid())
            .As(Enum.Parse<LicenseType>("Standard"))
            .ExpiresAt(DateTime.UtcNow.AddMonths(10))
            .WithAdditionalAttributes(additionals)
            .WithProductFeatures(feature)
            .LicensedTo(user.FirstName, user.Email)
            .CreateAndSignWithPrivateKey(privateKey, passPhrase);

        string licenceXMLDocument = CreateLicenceDocument(generatedLicense.ToString(), publicKey);
    }

    /// <summary>
    /// xml formatında alınana lisansı doğrulamak için kullanılır. 
    /// </summary>
    /// <param name="licenceText"></param>
    /// <returns></returns>
    public bool Validate(string licenceText)
    {
        XmlDocument document = new XmlDocument();
        document.LoadXml(licenceText);
        XmlNode? licenseXml = document.SelectSingleNode("LicenseXml");
        string licenseKey = licenseXml.SelectSingleNode("LicenseKey").InnerXml;
        string publicKey = licenseXml.SelectSingleNode("PublicKey").InnerText;


        License license = License.Load(licenseKey);
        IEnumerable<IValidationFailure> validationFailures = license.Validate()
                .ExpirationDate()
                .And()
               .Signature(publicKey)
               .AssertValidLicense();


        if (validationFailures.Any())
        {
            StringBuilder errorMessages = new StringBuilder();
            foreach (var item in validationFailures)
                errorMessages.Append(item.Message);

            return false;
        }
        return true;

    }

    /// <summary>
    ///  verilen lisans bilgilerini XML formatına döüştürür ve string olarak döner. 
    /// </summary>
    /// <param name="lisansKey"></param>
    /// <param name="publicKey"></param>
    /// <returns></returns>
    private string CreateLicenceDocument(string lisansKey, string publicKey)
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement root = xmlDocument.CreateElement("LicenseXml");
        xmlDocument.AppendChild(root);

        XmlElement publicKeyElement = xmlDocument.CreateElement("PublicKey");
        publicKeyElement.InnerText = publicKey;
        root.AppendChild(publicKeyElement);

        XmlElement licenseKeyElement = xmlDocument.CreateElement("LicenseKey");
        licenseKeyElement.InnerXml = lisansKey;
        root.AppendChild(licenseKeyElement);

        string licenceXMLDocument = xmlDocument.InnerXml;
        return licenceXMLDocument;
    }
}
