import * as grpc from '@grpc/grpc-js';
import { GreeterClient } from './proto/helloworld';

function main() {
  const client = new GreeterClient(
    'localhost:50051',
    grpc.credentials.createInsecure()
  );

  client.sayHello({ name: 'World' }, (error, response) => {
    if (error) {
      console.error(error);
      return;
    }
    console.log('Greeting:', response.message);
    
    // Call the second method
    client.sayHelloAgain({ name: 'World' }, (error, response) => {
      if (error) {
        console.error(error);
        return;
      }
      console.log('Greeting again:', response.message);
    });
  });
}

main();