using Grpc.Core;
using System.Threading.Tasks;

namespace GreeterGrpcServices.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        string instanceIp = await InstanceMetadata.GetInstanceIpAsync();
        return new HelloReply
        {
            Message = $"Hello, {request.Name}! From EC2 instance: {instanceIp}"
        };
    }

    public override async Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
    {
        string instanceIp = await InstanceMetadata.GetInstanceIpAsync();
        return new HelloReply
        {
            Message = $"Hello again, {request.Name}! From EC2 instance: {instanceIp}"
        };
    }
    
    public override async Task<HealthCheckResponse> HealthCheck(HealthCheckRequest request, ServerCallContext context)
    {
        _logger.LogInformation("ALB Health check requested");
        string instanceIp = await InstanceMetadata.GetInstanceIpAsync();
        _logger.LogInformation($"Health check from EC2 instance: {instanceIp}");
        return new HealthCheckResponse();
    }
}