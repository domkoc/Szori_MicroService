using Grpc.Core;
using Grpc.Net.ClientFactory;
using Streaming;

namespace MovieStream_FIBRPN
{
    public class MovieStreamService: MovieStream.MovieStreamBase
    {
        private readonly List<SingleMovie.SingleMovieClient> backends;
        public MovieStreamService(GrpcClientFactory clientFactory)
        {
            backends = new();
            try
            {
                int i = 0;
                while (true)
                {
                    backends.Add(clientFactory.CreateClient<SingleMovie.SingleMovieClient>($"SingleMovieClient{i}"));
                    i++;
                }
            } catch { }
        }
        public override async Task<GetMoviesReply> GetMovies(GetMoviesRequest request, ServerCallContext context)
        {
            var reply = new GetMoviesReply();
            foreach (var backend in backends)
            {
                var titleReply = await backend.GetTitleAsync(new GetTitleRequest());
                var lengthReply = await backend.GetLengthAsync(new GetLengthRequest());
                reply.Movies.Add(new MovieInfo() { Title = titleReply.Title, Length = lengthReply.Length });
            }
            return reply;
        }
        public override async Task Watch(WatchRequest request, IServerStreamWriter<WatchReply> responseStream, ServerCallContext context)
        {
            foreach (var backend in backends)
            {
                var titleReply = await backend.GetTitleAsync(new GetTitleRequest());
                if (titleReply.Title == request.Title)
                {
                    var framesReply = await backend.GetFramesAsync(new GetFramesRequest());
                    for (int i = request.StartPosition; i < framesReply.Frame.Count; i++)
                    {
                        await responseStream.WriteAsync(new WatchReply() { Frame = framesReply.Frame[i] } );
                    }
                    break;
                }
            }
        }
    }
}
