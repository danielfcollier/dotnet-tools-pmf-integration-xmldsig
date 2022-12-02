#nullable disable

using System.Xml;
using System.Xml.Serialization;

namespace Models;

[XmlRoot("xmlProcessamentoNfpse")]
public class InvoiceRequest
{
    [XmlElement(ElementName = "razaoSocialTomador")]
    public string RazaoSocialTomador { get; set; }

    [XmlElement(ElementName = "emailTomador")]
    public string EmailTomador { get; set; }

    [XmlElement(ElementName = "identificacaoTomador")]
    public string IdentificacaoTomador { get; set; }

    [XmlElement(ElementName = "telefoneTomador")]
    public string? TelefoneTomador { get; set; }

    [XmlElement(ElementName = "inscricaoMunicipalTomador")]
    public string? InscricaoMunicipalTomador { get; set; }

    [XmlElement(ElementName = "logradouroTomador")]
    public string LogradouroTomador { get; set; }

    [XmlElement(ElementName = "bairroTomador")]
    public string BairroTomador { get; set; }

    [XmlElement(ElementName = "codigoPostalTomador")]
    public string CodigoPostalTomador { get; set; }

    [XmlElement(ElementName = "ufTomador")]
    public string UfTomador { get; set; }

    [XmlElement(ElementName = "complementoEnderecoTomador")]
    public string? ComplementoEnderecoTomador { get; set; }

    [XmlElement(ElementName = "dadosAdicionais")]
    public string? DadosAdicionais { get; set; }

    [XmlElement(ElementName = "codigoMunicipioTomador")]
    public int? CodigoMunicipioTomador { get; set; }

    [XmlElement(ElementName = "numeroEnderecoTomador")]
    public string? NumeroEnderecoTomador { get; set; }

    [XmlElement(ElementName = "paisTomador")]
    public int? PaisTomador { get; set; }

    [XmlElement(ElementName = "dataEmissao")]
    public string DataEmissao { get; set; }

    [XmlElement(ElementName = "valorTotalServicos")]
    public decimal ValorTotalServicos { get; set; }

    [XmlElement(ElementName = "baseCalculo")]
    public decimal BaseCalculo { get; set; }

    [XmlElement(ElementName = "baseCalculoSubstituicao")]
    public decimal? BaseCalculoSubstituicao { get; set; }

    [XmlElement(ElementName = "valorISSQN")]
    public decimal ValorIssqn { get; set; }

    [XmlElement(ElementName = "valorISSQNSubstituicao")]
    public decimal? ValorIssqnSubstituicao { get; set; }

    [XmlElement(ElementName = "identificacao")]
    public string Identificacao { get; set; }

    [XmlElement(ElementName = "numeroAEDF")]
    public string NumeroAedf { get; set; }

    [XmlElement(ElementName = "cfps")]
    public int Cfps { get; set; }

    [XmlArray("itensServico")]
    [XmlArrayItem("itemServico")]
    public List<ItemServico> ItensServico { get; set; }

    public class ItemServico
    {
        [XmlElement(ElementName = "aliquota")]
        public decimal Aliquota { get; set; }

        [XmlElement(ElementName = "baseCalculo")]
        public decimal BaseCalculo { get; set; }

        [XmlElement(ElementName = "cst")]
        public int Cst { get; set; }

        [XmlElement(ElementName = "descricaoServico")]
        public string DescricaoServico { get; set; }

        [XmlElement(ElementName = "idCNAE")]
        public int IdCnae { get; set; }

        [XmlElement(ElementName = "quantidade")]
        public decimal Quantidade { get; set; }

        [XmlElement(ElementName = "valorTotal")]
        public decimal ValorTotal { get; set; }

        [XmlElement(ElementName = "valorUnitario")]
        public decimal ValorUnitario { get; set; }
    }
}