import * as grpc from '@grpc/grpc-js';
import { GreeterService } from './proto/helloworld';
import { HelloReply } from './proto/helloworld';
import { health } from '@grpc/health-check';

const host = '0.0.0.0:50051';

const sayHello = (call: any, callback: any) => {
  const reply: HelloReply = { message: `Hello, ${call.request.name}!` };
  callback(null, reply);
};

const sayHelloAgain = (call: any, callback: any) => {
  const reply: HelloReply = { message: `Hello again, ${call.request.name}!` };
  callback(null, reply);
};

function main() {
  const server = new grpc.Server();
  
  // Add the greeter service
  server.addService(GreeterService, {
    sayHello,
    sayHelloAgain
  });
  
  // Add health service
  const healthImpl = new health.HealthImplementation();
  server.addService(health.service, healthImpl);
  healthImpl.setStatus('', health.servingStatus.SERVING);
  
  server.bindAsync(
    host,
    grpc.ServerCredentials.createInsecure(),
    (err, port) => {
      if (err) {
        console.error(`Server error: ${err.message}`);
        return;
      }
      console.log(`Server running at ${host}`);
      server.start();
    }
  );
}

main();