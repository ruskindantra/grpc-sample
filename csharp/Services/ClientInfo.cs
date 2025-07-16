using Grpc.Core;
using System.Collections.Generic;
using System.Text;

namespace GreeterGrpcServices.Services;

public static class ClientInfo
{
    public static string GetClientInfo(ServerCallContext context)
    {
        var sb = new StringBuilder();
        
        // Get direct peer address
        string peerAddress = context.Peer;
        sb.AppendLine($"Direct peer: {peerAddress}");

        // Check for X-Forwarded-For header (used by ALB and CloudFront)
        var forwardedFor = context.RequestHeaders.Get("x-forwarded-for");
        if (forwardedFor != null)
        {
            sb.AppendLine($"X-Forwarded-For: {forwardedFor}");
        }
        
        // Check for CloudFront specific headers
        var cfId = context.RequestHeaders.Get("x-amz-cf-id");
        if (cfId != null)
        {
            sb.AppendLine($"CloudFront Request ID: {cfId}");
        }
        
        // Get all headers that might contain client information
        var relevantHeaders = new List<string> 
        { 
            "x-real-ip", 
            "x-client-ip", 
            "true-client-ip",
            "x-cluster-client-ip",
            "forwarded"
        };
        
        foreach (var header in relevantHeaders)
        {
            var value = context.RequestHeaders.Get(header);
            if (value != null)
            {
                sb.AppendLine($"{header}: {value}");
            }
        }
        
        return sb.ToString();
    }
}