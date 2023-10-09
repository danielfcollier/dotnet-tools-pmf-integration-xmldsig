using System.Net;
using System.Xml;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using CommandLine;
using Handlers;
using Services;
using Models;

public partial class Program
{
    public class ArgOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('i', "id", Required = false, HelpText = "Partner Id.")]
        public string Partner { get; set; }

        [Option('f', "inputFile", Required = false, HelpText = "Input file.")]
        public string InputFile { get; set; }
        
        [Option('o', "outputFile", Required = false, HelpText = "Output file.")]
        public string OutputFile { get; set; }
    }

    private static async Task Main(string[] args)
    {
        if (!validateArgs(args))
        {
            return;
        }

        ArgOptions? options = null;
        IEnumerable<CommandLine.Error>? errors = null;

        Parser.Default.ParseArguments<ArgOptions>(args)
            .WithParsed(opts => options = opts)
            .WithNotParsed((errs) => errors = errs);

        if (options is null)
        {
            HandleParseError(errors);
            return;
        }

        // TODO: remove default parameters
        options.Partner = "#000.001";
        options.InputFile = Path.Join(".", "Db", "data.csv");
        options.OutputFile = Path.Join(".", "Db", "output.csv");
        
        await RunOptionsAndReturnExitCode(options);
    }

    private static bool validateArgs(string[] args)
    {
        foreach (var arg in args)
        {
            if (!Regex.IsMatch(arg, @"^-?[a-zA-Z0-9]{1,40}$"))
            {
                return false;
            }
        }

        return true;
    }

    private static void HandleParseError(IEnumerable<CommandLine.Error>? errs)
    {
        if (errs is not null)
        {
            foreach (var err in errs)
            {
                Console.WriteLine(err.Tag.ToString());
            }
        }
    }

    // Main Program
    private static async Task RunOptionsAndReturnExitCode(ArgOptions options)
    {
        /*
        CLI: partnerId inputFile [outputFile]
        Logs: for whom is processing, what is processing, status
        */
        string partnerId = options.Partner;
        string filepath = Path.Join("partnersData.json");
        Partner partner = await DBHandler.GetPartnerData(filepath, partnerId);


        // Start
        Console.WriteLine($"Processing for partner {partnerId}");

        // Services
        XmlDSigService signer = new (partner.Secrets);
        InvoiceService invoiceService = new(partner);

        // Produce
        List<Invoice> invoices = CSVHandler.Read<Invoice>(options.InputFile);
        List<Invoice> processedInvoices = new ();

        // Consume
        foreach (var invoice in invoices)
        {
            bool hasBeenSent = false;

            if (!invoice.IsSent)
            {
                InvoiceRequest request = await invoiceService.MountRequest(invoice);
                XmlDocument xmlRequest = signer.SignInvoice(request);

                var (response, statusCode) = await Run(invoiceService, invoice.Id, xmlRequest);

                hasBeenSent = statusCode == HttpStatusCode.OK;
            }

            if (!hasBeenSent)
            {
                processedInvoices.Add(invoice);
            }
            else
            {
                Invoice updatedInvoice = new ()
                {
                    Id = invoice.Id,
                    ExternalId = invoice.ExternalId,
                    PaymentId = invoice.PaymentId,
                    DocumentId = invoice.DocumentId,
                    FullName = invoice.FullName,
                    Birthday = invoice.Birthday, // TODO: remove
                    Email = invoice.Email,
                    Cellphone = invoice.Cellphone,
                    PostalCode = invoice.PostalCode,
                    ConsultationDate = invoice.ConsultationDate,  // TODO: remove
                    Description = invoice.Description,  // TODO: remove
                    ConsultationValue = invoice.ConsultationValue,  // TODO: remove
                    Services = invoice.Services,
                    CmcCode = invoice.CmcCode,
                    VerificationCode = invoice.VerificationCode,
                    Observation = invoice.Observation,
                    EffectiveDate = invoice.EffectiveDate,   // TODO: change do ProcessingDate
                    InvoiceDate = invoice.InvoiceDate,
                    IsSent = true,
                };
                processedInvoices.Add(updatedInvoice);
            }
        }

        CSVHandler.Write<Invoice>(options.OutputFile, processedInvoices);
        Console.WriteLine("### DONE!!!");
    }

    private static async Task<(string?, HttpStatusCode)> Run(InvoiceService invoiceService, string id, XmlDocument request)
    {
        Console.Write($"Processing request: {id}");
        
        var (response, statusCode) = await invoiceService.Request(request);
        var message = statusCode == HttpStatusCode.OK ? "SUCCESS" : "FAILED";

        Console.Write($" - {message}\n");
        return (response, statusCode);
    }

    private static async Task<(string?, HttpStatusCode)> Validate(InvoiceService invoiceService, string id, XmlDocument request)
    {
        Console.Write($"Validating request: {id}");
        
        var (response, statusCode) = await invoiceService.Validate(request);
        var message = statusCode == HttpStatusCode.OK ? "SUCCESS" : "FAILED";

        Console.Write($" - {message}\n");
        return (response, statusCode);
    }
}