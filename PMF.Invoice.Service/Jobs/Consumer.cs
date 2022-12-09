using System.Net;
using System.Xml;

using Models;

using Services;

namespace Jobs;

public class Consumer
{
    private InvoiceService invoiceService;

    public Consumer(Partner partner)
    {
        invoiceService = new(partner);
    }

    public async Task<(string?, HttpStatusCode)> Run(string id, XmlDocument payload)
    {
        Console.WriteLine($"Processing payload: {id}");

        return await invoiceService.Request(payload);
    }

    public async Task<(string?, HttpStatusCode)> Validate(string id, XmlDocument payload)
    {
        Console.WriteLine($"Processing payload: {id}");

        return await invoiceService.Validate(payload);
    }


    // public async Task Save(InvoiceResponse response)
    // {
    // }
}