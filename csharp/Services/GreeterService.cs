using Grpc.Core;

namespace GreeterGrpcServices.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello, {request.Name}!"
        });
    }

    public override Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello again, {request.Name}!"
        });
    }
    
    public override Task<HealthCheckResponse> HealthCheck(HealthCheckRequest request, ServerCallContext context)
    {
        _logger.LogInformation("ALB Health check requested");
        return Task.FromResult(new HealthCheckResponse());
    }
}