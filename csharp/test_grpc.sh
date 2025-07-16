#!/bin/bash

# Install grpcurl if not already installed
if ! command -v grpcurl &> /dev/null; then
    echo "Installing grpcurl..."
    sudo dnf install -y golang
    go install github.com/fullstorydev/grpcurl/cmd/grpcurl@latest
    export PATH=$PATH:~/go/bin
fi

echo "Testing gRPC reflection..."
grpcurl -plaintext localhost:8080 list

echo -e "\nTesting gRPC health check..."
grpcurl -plaintext -d '{}' localhost:8080 AWS.ALB.healthcheck/Check