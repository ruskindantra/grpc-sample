#!/bin/bash

# Update system packages
sudo dnf update -y

# Install AWS CLI if not already installed
if ! command -v aws &> /dev/null; then
    sudo dnf install -y aws-cli
fi

# Install ICU libraries for .NET globalization support
sudo dnf install -y libicu

# Ensure firewall allows port 8080
sudo dnf install -y firewalld
sudo systemctl start firewalld
sudo systemctl enable firewalld
sudo firewall-cmd --permanent --add-port=8080/tcp
sudo firewall-cmd --permanent --add-port=8081/tcp
sudo firewall-cmd --reload

# Create application directory
sudo mkdir -p /opt/grpc-app
sudo chown ec2-user:ec2-user /opt/grpc-app

# Download the application package from S3
aws s3 cp s3://grpc-sample-deploy/grpc-app.zip ~/grpc-app.zip

# Extract the downloaded zip file to the application directory
unzip -o ~/grpc-app.zip -d /opt/grpc-app

# List files in the app directory
ls -la /opt/grpc-app/

# Create a systemd service file for the application
cat << EOF | sudo tee /etc/systemd/system/grpc-app.service
[Unit]
Description=gRPC Application Service
After=network.target

[Service]
User=ec2-user
WorkingDirectory=/opt/grpc-app
ExecStart=/opt/grpc-app/GreeterGrpcServices
Restart=always
RestartSec=3
Environment="ASPNETCORE_URLS=http://0.0.0.0:8080"
Environment="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1"
Environment="ASPNETCORE_HTTP2UNENCRYPTEDSUPPORT=true"

[Install]
WantedBy=multi-user.target
EOF

# Stop, disable, and then enable and start the service
sudo systemctl stop grpc-app
sudo systemctl disable grpc-app
sudo systemctl daemon-reload
sudo systemctl enable grpc-app
sudo systemctl start grpc-app

# Wait a moment for the service to start
sleep 2

# Print status and logs
sudo systemctl status grpc-app
echo "\n--- Recent logs ---"
sudo journalctl -u grpc-app -n 20 --no-pager