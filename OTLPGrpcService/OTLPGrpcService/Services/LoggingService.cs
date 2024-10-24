using Grpc.Core;
using OpenTelemetry.Proto.Collector.Logs.V1;

namespace OTLPGrpcService.Services
{
    internal class LoggingService : LogsService.LogsServiceBase
    {
        public override async Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
        {
            foreach (var resourceLog in request.ResourceLogs)
            {
                foreach (var scopeLog in resourceLog.ScopeLogs)
                {
                    foreach (var logRecord in scopeLog.LogRecords)
                    {
                        var message = logRecord.Body.StringValue;
                        Console.WriteLine($"Received log: {message}");
                    }
                }
            }

            return new ExportLogsServiceResponse();
        }
    }
}
