# TypeScript gRPC Application for EC2 and ALB

This is a simple gRPC application in TypeScript that can be deployed on an EC2 instance and accessed through an Application Load Balancer (ALB).

## Local Setup

1. Install Node.js (v14+) and npm
2. Install protobuf compiler:
   - macOS: `brew install protobuf`
   - Linux: `apt-get install protobuf-compiler` or `yum install protobuf-compiler`

3. Install dependencies:
   ```
   npm install
   ```

4. Build the application:
   ```
   npm run build
   ```

5. Run the server:
   ```
   npm start
   ```

6. In another terminal, run the client to test:
   ```
   npm run client
   ```

## EC2 Deployment

1. Launch an EC2 instance with Amazon Linux 2
2. Make sure the security group allows inbound traffic on port 50051
3. Upload the code to the instance or clone from your repository
4. Run the setup script:
   ```
   chmod +x ec2_setup.sh
   ./ec2_setup.sh
   ```

## ALB Setup

1. Create a target group:
   - Target type: Instances
   - Protocol: HTTP
   - Port: 50051
   - Health check path: /
   - Register your EC2 instance

2. Create an Application Load Balancer:
   - Listeners: HTTP on port 80
   - Target group: The one you created above

3. Configure security groups to allow traffic from the ALB to the EC2 instance

## Notes

- The gRPC server runs on port 50051
- The health check service is available on the same port
- The @grpc/health-check package provides a standard health check implementation

## Production Considerations

For a production environment, consider:
- Using HTTPS/TLS for secure communication
- Implementing proper authentication
- Setting up auto-scaling for EC2 instances
- Using AWS Fargate or ECS for containerized deployment