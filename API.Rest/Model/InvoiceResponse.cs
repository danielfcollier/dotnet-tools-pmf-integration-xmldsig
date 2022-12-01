#nullable disable

using System.Xml;
using System.Xml.Serialization;

namespace Model;

[XmlRoot("xmlNfpse")]
public class InvoiceResponse
{
    [XmlElement(ElementName = "numeroSerie")]
    public long NumeroSerie { get; set; }

    [XmlElement(ElementName = "numeroAEDF")]
    public string NumeroAEDF { get; set; }

    [XmlElement(ElementName = "codigoVerificacao")]
    public string CodigoVerificacao { get; set; }

    [XmlElement(ElementName = "dataEmissao")]
    public DateTime DataEmissao { get; set; }

    [XmlElement(ElementName = "dataProcessamento")]
    public string DataProcessamento { get; set; }

    [XmlElement(ElementName = "statusNFPSe")]
    public int StatusNfspe { get; set; }

    [XmlElement(ElementName = "homologacao")]
    public bool? Homologacao { get; set; }
    [XmlElement(ElementName = "cnpjPrestador")]
    public string CnpjPrestador { get; set; }

    [XmlElement(ElementName = "bairroPrestador")]
    public string BairroPrestador { get; set; }

    [XmlElement(ElementName = "nomeMunicipioPrestador")]
    public string NomeMunicipioPrestador { get; set; }

    [XmlElement(ElementName = "ufPrestador")]
    public string UfPrestador { get; set; }

    [XmlElement(ElementName = "codigoPostalPrestador")]
    public string CodigoPostalPrestador { get; set; }

    [XmlElement(ElementName = "nomeMunicipioTomador")]
    public string NomeMunicipioTomador { get; set; }

    [XmlElement(ElementName = "razaoSocialPrestador")]
    public string RazaoSocialPrestador { get; set; }

    [XmlElement(ElementName = "logradouroPrestador")]
    public string LogradouroPrestador { get; set; }

    [XmlElement(ElementName = "telefonePrestador")]
    public string TelefonePrestador { get; set; }

    [XmlElement(ElementName = "identificacao")]
    public string Identificacao { get; set; }

    [XmlElement(ElementName = "inscricaoMunicipalPrestador")]
    public string InscricaoMunicipalPrestador { get; set; }

    [XmlElement(ElementName = "DataEmissao")]
    public string dataEmissao { get; set; }

    [XmlArray("itensServico")]
    [XmlArrayItem("itemServico")]
    public List<ItemServico> ItensServico { get; set; }
    public class ItemServico
    {
        [XmlElement(ElementName = "codigoCNAE")]
        public string CodigoCNAE { get; set; }

        [XmlElement(ElementName = "descricaoCNAE")]
        public string DescricaoCNAE { get; set; }
    }
}