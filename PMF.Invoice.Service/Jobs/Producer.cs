using System.Xml;

using Handlers;

using Models;

using Services;

namespace Jobs;

public class Producer
{
    private Partner partner;
    private XmlDSigService signer;

    public Producer(Partner _partner)
    {
        partner = _partner;
        signer = new XmlDSigService(partner.Secrets);
    }

    public async Task<List<(XmlDocument, string)>> GetPayloadsToProcess()
    {
        var results = new List<(XmlDocument, string)>();

        string filepath = Path.Join(".", "Db", "data.csv");
        List<Customer> customers = CSVHandler.Read<Customer>(filepath);

        foreach (var customer in customers)
        {
            var result = await GetPayload(customer);

            results.Add(result);
        }

        return results;
    }

    public async Task<(XmlDocument, string)> GetPayload(Customer customer)
    {
        InvoiceRequest request = await MountPayload(customer);
        XmlDocument payload = signer.SignInvoice(request);

        return (payload, customer.Id);
    }

    private async Task<InvoiceRequest> MountPayload(Customer customer)
    {
        List<InvoiceRequest.ItemServico> itensServico = InvoiceService.GetItensServico(partner, customer);
        var (baseCalculo, valorIssqn, valorTotalServicos) = InvoiceService.GetTotals(itensServico);

        Address address = await AddressService.Get(customer.PostalCode, partner.Municipal.FallbackAddress);

        InvoiceRequest result = new()
        {
            // Identificacao = "#0001", // TODO: string 10 chars
            NumeroAedf = Globals.IS_PROD ? partner.Municipal.NumeroAedf : partner.Secrets.Login.User.Remove(partner.Secrets.Login.User.Length - 1, 1),
            DataEmissao = UtilsHandler.MaskDate(customer.EffectiveDate),
            //
            ItensServico = itensServico,
            //
            BaseCalculo = baseCalculo,
            ValorIssqn = valorIssqn,
            ValorTotalServicos = valorTotalServicos,
            DadosAdicionais = MountAdditionalData(), // TODO
            //
            RazaoSocialTomador = customer.FullName,
            EmailTomador = Globals.IS_PROD ? customer.Email : partner.Secrets.Email,
            IdentificacaoTomador = UtilsHandler.MaskPersonDocument(customer.DocumentId),
            //
            PaisTomador = address.CountryCode,
            CodigoMunicipioTomador = Int32.Parse(address.MunicipalCode),
            LogradouroTomador = address.Street,
            NumeroEnderecoTomador = "-",
            BairroTomador = address.Neighborhood,
            CodigoPostalTomador = UtilsHandler.OnlyNumbers(address.PostalCode),
            UfTomador = address.State,
            Cfps = CityCodesService.GetCfps(Int32.Parse(address.MunicipalCode)),
        };

        return result;
    }

    private string MountAdditionalData()
    {
        return partner.Additional.Message;
    }
}