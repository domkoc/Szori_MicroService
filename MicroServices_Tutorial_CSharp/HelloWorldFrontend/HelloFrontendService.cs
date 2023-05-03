using Grpc.Core;
using HelloWorldMicroService;
namespace HelloWorldFrontend
{
    public class HelloFrontendService : Hello.HelloBase
    {
        private readonly Hello.HelloClient _backend;
        public HelloFrontendService(Hello.HelloClient client)
        {
            _backend = client;
        }
        public override async Task<SayHelloReply> SayHello(SayHelloRequest request, ServerCallContext context)
        {
            var replyFromBackend = await _backend.SayHelloAsync(request);
            return
            new SayHelloReply()
            {
                Message = $"Hello from frontend: {replyFromBackend.Message}"
            };
        }
    }
}