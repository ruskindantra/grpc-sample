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

echo -e "\nTesting SayHello method..."
grpcurl -plaintext -d '{"name": "World"}' localhost:8080 greeterpackage.Greeter/SayHello

echo -e "\nTesting SayHelloAgain method..."
grpcurl -plaintext -d '{"name": "Greeter"}' localhost:8080 greeterpackage.Greeter/SayHelloAgain

echo -e "\nTesting HealthCheck method..."
grpcurl -plaintext -d '{}' localhost:8080 greeterpackage.Greeter/HealthCheck