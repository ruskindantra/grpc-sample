using System.Net.Http;
using System.Threading.Tasks;

namespace GreeterGrpcServices.Services;

public static class InstanceMetadata
{
    private static string? _instanceIp;

    public static async Task<string> GetInstanceIpAsync()
    {
        if (!string.IsNullOrEmpty(_instanceIp))
        {
            return _instanceIp;
        }
        
        try
        {
            using var httpClient = new HttpClient();
            
            // Get IMDSv2 token first
            var tokenRequest = new HttpRequestMessage(HttpMethod.Put, "http://169.254.169.254/latest/api/token");
            tokenRequest.Headers.Add("X-aws-ec2-metadata-token-ttl-seconds", "21600");
            var tokenResponse = await httpClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var token = await tokenResponse.Content.ReadAsStringAsync();
            
            // Use token to get instance IP
            var ipRequest = new HttpRequestMessage(HttpMethod.Get, "http://169.254.169.254/latest/meta-data/local-ipv4");
            ipRequest.Headers.Add("X-aws-ec2-metadata-token", token);
            var ipResponse = await httpClient.SendAsync(ipRequest);
            ipResponse.EnsureSuccessStatusCode();
            _instanceIp = await ipResponse.Content.ReadAsStringAsync();
            
            return _instanceIp;
        }
        catch
        {
            // Return a fallback value if not running on EC2 or metadata service is unavailable
            return "Not running on EC2 or metadata unavailable";
        }
    }
}