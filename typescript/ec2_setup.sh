#!/bin/bash

# Update system packages
sudo yum update -y

# Install Node.js
curl -sL https://rpm.nodesource.com/setup_16.x | sudo bash -
sudo yum install -y nodejs

# Install git
sudo yum install -y git

# Install protobuf compiler
sudo yum install -y protobuf-compiler

# Clone the repository (replace with your actual repository URL)
git clone https://your-repo-url.git /home/ec2-user/grpc-app
cd /home/ec2-user/grpc-app/typescript

# Install dependencies
npm install

# Build the application
npm run build

# Create a systemd service file for the application
cat << EOF | sudo tee /etc/systemd/system/grpc-app.service
[Unit]
Description=gRPC Application Service
After=network.target

[Service]
User=ec2-user
WorkingDirectory=/home/ec2-user/grpc-app/typescript
ExecStart=/usr/bin/node dist/server.js
Restart=always
RestartSec=3

[Install]
WantedBy=multi-user.target
EOF

# Enable and start the service
sudo systemctl enable grpc-app
sudo systemctl start grpc-app

# Print status
sudo systemctl status grpc-app