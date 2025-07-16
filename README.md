# gRPC Application for EC2 and ALB

This repository contains simple gRPC applications that can be deployed on an EC2 instance and accessed through an Application Load Balancer (ALB). The applications are implemented in multiple languages:

- [C# Implementation](./csharp/README.md)
- [TypeScript Implementation](./typescript/README.md)

## Overview

Each implementation provides:
- A basic gRPC service with two methods: `SayHello` and `SayHelloAgain`
- Health check endpoints for ALB integration
- Setup scripts for EC2 deployment
- Instructions for ALB configuration

## EC2 Deployment

1. Choose your preferred implementation (C# or TypeScript)
2. Launch an EC2 instance with Amazon Linux 2023
3. Make sure the security group allows inbound traffic on port 8080
4. Make sure the EC2 instance has an IAM role with S3 access permissions
5. Build and upload the application to S3 using the deployment script
6. SSH into your EC2 instance and run the setup script

## ALB Setup

1. Create a target group:
   - Target type: Instances
   - Protocol: HTTP
   - Port: 8080
   - Health check path: `/` or `/healthz` (depending on implementation)
   - Register your EC2 instance

2. Create an Application Load Balancer:
   - Listeners: HTTP on port 80
   - Target group: The one you created above

3. Configure security groups to allow traffic from the ALB to the EC2 instance

## Notes

- The gRPC server runs on port 8080 (in production)
- Health check endpoints are available on the same port
- The ALB will route traffic to the health check endpoint, but your gRPC clients should connect directly to the EC2 instance's public IP or DNS on port 50051

## Production Considerations

For a production environment, consider:
- Using HTTPS/TLS for secure communication
- Implementing proper authentication
- Setting up auto-scaling for EC2 instances
- Using AWS Fargate or ECS for containerized deployment