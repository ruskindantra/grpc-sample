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
        string clientInfo = ClientInfo.GetClientInfo(context);
        
        return new HelloReply
        {
            Message = $"Hello, {request.Name}!\nFrom EC2 instance: {instanceIp}\nClient Info:\n{clientInfo}"
        };
    }

    public override async Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
    {
        string instanceIp = await InstanceMetadata.GetInstanceIpAsync();
        string clientInfo = ClientInfo.GetClientInfo(context);
        
        return new HelloReply
        {
            Message = $"Hello again, {request.Name}!\nFrom EC2 instance: {instanceIp}\nClient Info:\n{clientInfo}"
        };
    }
    
    public override async Task<HealthCheckResponse> HealthCheck(HealthCheckRequest request, ServerCallContext context)
    {
        string instanceIp = await InstanceMetadata.GetInstanceIpAsync();
        string clientInfo = ClientInfo.GetClientInfo(context);
        
        _logger.LogInformation($"Health check from EC2 instance: {instanceIp}");
        _logger.LogInformation($"Client info: {clientInfo}");
        
        return new HealthCheckResponse();
    }
}