# C# gRPC Application for EC2 and ALB

This is a simple gRPC application in C# that can be deployed on an EC2 instance and accessed through an Application Load Balancer (ALB).

## Local Setup

1. Install .NET 8.0 SDK:
   - Download from https://dotnet.microsoft.com/download/dotnet/8.0

2. Build and run the application:
   ```
   dotnet build
   dotnet run
   ```

3. The gRPC server will start on a dynamically assigned port (which will be displayed in the console) with health check endpoints available at:
   - HTTP health check: http://localhost:[PORT]/healthz
   - gRPC health check: available through gRPC protocol on the same port

## EC2 Deployment

### Prerequisites

1. AWS CLI installed and configured with appropriate permissions to access the S3 bucket
2. An S3 bucket named `grpc-sample-deploy`

### Deployment Steps

1. Create and upload the deployment package to S3:
   ```
   chmod +x create_deployment.sh
   ./create_deployment.sh
   ```

2. Launch an EC2 instance with Amazon Linux 2023

3. Make sure the security group allows inbound traffic on port 8080

4. Make sure the EC2 instance has an IAM role with permissions to access the S3 bucket

5. SSH into your EC2 instance:
   ```
   ssh -i your-key.pem ec2-user@your-ec2-ip
   ```

6. Download and run the setup script:
   ```
   aws s3 cp s3://grpc-sample-deploy/ec2_setup.sh .
   chmod +x ec2_setup.sh
   ./ec2_setup.sh
   ```

7. Verify the service is running:
   ```
   sudo systemctl status grpc-app
   ```

## ALB Setup

1. Create a target group:
   - Target type: Instances
   - Protocol: gRPC
   - Port: 8080
   - Health check path: /greeterpackage.Greeter/HealthCheck
   - Health check protocol: gRPC
   - Success codes: 0
   - Register your EC2 instance

2. Create an Application Load Balancer:
   - Listeners: HTTP on port 80
   - Target group: The one you created above

3. Configure security groups to allow traffic from the ALB to the EC2 instance

## Notes

- The gRPC server runs on port 8080 in production
- The health check endpoint is available at /greeterpackage.Greeter/HealthCheck
- The service includes three methods: SayHello, SayHelloAgain, and HealthCheck

## Production Considerations

For a production environment, consider:
- Using HTTPS/TLS for secure communication
- Implementing proper authentication
- Setting up auto-scaling for EC2 instances
- Using AWS Fargate or ECS for containerized deployment