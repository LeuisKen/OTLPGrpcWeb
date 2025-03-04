using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using System;

namespace OTLPConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddOpenTelemetry(logging =>
                {
                    logging.AddOtlpExporter((exporterOptions, processorOptions) =>
                    {
                        exporterOptions.Protocol = OtlpExportProtocol.Grpc;
                        exporterOptions.Endpoint = new Uri("http://localhost:4317");
                        processorOptions.ExportProcessorType = ExportProcessorType.Simple;
                    });
                });
            });

            var logger = loggerFactory.CreateLogger<Program>();

            logger.FoodPriceChanged("artichoke", 9.99);

            logger.FoodRecallNotice(
                brandName: "Contoso",
                productDescription: "Salads",
                productType: "Food & Beverages",
                recallReasonDescription: "due to a possible health risk from Listeria monocytogenes",
                companyName: "Contoso Fresh Vegetables, Inc.");

            // Dispose logger factory before the application ends.
            // This will flush the remaining logs and shutdown the logging pipeline.
            loggerFactory.Dispose();
        }
    }

    internal static class LoggerExtensions
    {
        public static void FoodPriceChanged(this ILogger logger, string name, double price)
        {
            logger.LogInformation("Food `{name}` price changed to `{price}`.", name, price);
        }

        public static void FoodRecallNotice(
            this ILogger logger,
            string brandName,
            string productDescription,
            string productType,
            string recallReasonDescription,
            string companyName)
        {
            logger.LogCritical("A `{productType}` recall notice was published for `{brandName} {productDescription}` produced by `{companyName}` ({recallReasonDescription}).",
                productType, brandName, productDescription, companyName, recallReasonDescription);
        }
    }
}
