#!/bin/bash

# Build and publish the application as self-contained for Linux
dotnet publish -c Release -o ./publish --self-contained -r linux-x64

# Create a deployment package
cd ./publish
zip -r ../grpc-app.zip *
cd ..

# Upload the deployment package to S3
aws s3 cp grpc-app.zip s3://grpc-sample-deploy/

# Upload the EC2 setup script to S3
aws s3 cp ec2_setup.sh s3://grpc-sample-deploy/

# Upload the test scripts to S3
aws s3 cp test_health.sh s3://grpc-sample-deploy/
aws s3 cp test_grpc.sh s3://grpc-sample-deploy/

echo "Deployment package uploaded to S3: s3://grpc-sample-deploy/grpc-app.zip"
echo "EC2 setup script uploaded to S3: s3://grpc-sample-deploy/ec2_setup.sh"
echo ""
echo "Next steps:"
echo "1. SSH into your EC2 instance:"
echo "   ssh -i your-key.pem ec2-user@your-ec2-ip"
echo ""
echo "2. Download and run the setup script:"
echo "   aws s3 cp s3://grpc-sample-deploy/ec2_setup.sh ."
echo "   chmod +x ec2_setup.sh"
echo "   ./ec2_setup.sh"