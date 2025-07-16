using Grpc.Core;
using AWS.ALB;
using System.Threading.Tasks;

namespace GrpcService.Services;

public class AlbHealthCheckService : healthcheck.healthcheckBase
{
    private readonly ILogger<AlbHealthCheckService> _logger;

    public AlbHealthCheckService(ILogger<AlbHealthCheckService> logger)
    {
        _logger = logger;
    }

    public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
    {
        _logger.LogInformation("ALB Health check requested at /AWS.ALB/healthcheck");
        _logger.LogInformation($"Request from: {context.Peer}");
        
        try
        {
            // Log all headers for debugging
            foreach (var header in context.RequestHeaders)
            {
                _logger.LogInformation($"Header: {header.Key}={header.Value}");
            }
            
            // Return empty response with OK status
            return Task.FromResult(new HealthCheckResponse());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in health check");
            throw;
        }
    }
}