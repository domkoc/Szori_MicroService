using Grpc.Core;
using HelloWorldMicroService;

namespace HelloWorldBackend
{
    public class HelloBackendService: Hello.HelloBase
    {
        public override async Task<SayHelloReply> SayHello(SayHelloRequest request, ServerCallContext context)
        {
            return
            new SayHelloReply()
            {
                Message = $"Hello from backend: {request.Name}"
            };
        }
    }
}
