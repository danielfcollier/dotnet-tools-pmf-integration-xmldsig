using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;

using Handlers;
using Models;

namespace Services;

public class XmlDSigService
{
    private readonly X509Certificate2 certificate;

    public XmlDSigService(Credentials credentials)
    {
        certificate = GetCertificate(credentials);
    }

    public XmlDocument XmlDSig(XmlDocument payload)
    {
        XmlDocument result = (XmlDocument)payload.CloneNode(true);

        RSA? privateKey = certificate.GetRSAPrivateKey();

        SignedXml signedXml = new(result) { SigningKey = privateKey };
        signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";

        KeyInfo keyInfo = new();
        keyInfo.AddClause(new KeyInfoX509Data(certificate));
        signedXml.KeyInfo = keyInfo;

        Reference reference = new() { Uri = String.Empty };
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        reference.AddTransform(new XmlDsigExcC14NTransform());

        signedXml.AddReference(reference);
        signedXml.ComputeSignature();

        XmlElement? xmlDigitalSignature = signedXml.GetXml();
        result.DocumentElement!.AppendChild(result.ImportNode(xmlDigitalSignature, true));

        return result;
    }

    public bool IsValidXmlDSig(XmlDocument xml)
    {
        bool isValid;
        SignedXml signedXml = new(xml);

        var signatureElement = xml.GetElementsByTagName("Signature");
        if (signatureElement.Count != 1)
        {
            return false;
        }

        signedXml.LoadXml((XmlElement)signatureElement[0]!);

        if ((signedXml.SignedInfo.References[0] as Reference)?.Uri != String.Empty)
        {
            return false;
        }

        isValid = signedXml.CheckSignature(certificate.GetRSAPublicKey());

        return isValid;
    }

    public X509Certificate2 GetCertificate(Credentials credentials)
    {
        byte[] bytes = Convert.FromBase64String(credentials.Certificate.Data);

        return new X509Certificate2(bytes, credentials.Certificate.Password);
    }

    public XmlDocument SignInvoice(object invoice)
    {
        XmlDocument xmlInvoice = XmlHandler.SerializeObjectToXmlDocument(invoice);
        XmlDocument signedInvoice = XmlDSig(xmlInvoice);

        return signedInvoice;
    }
}