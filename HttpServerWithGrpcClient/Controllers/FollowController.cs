using Grpc.Net.Client;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;

public class RequestFollowData
{
    public int IdUser { get; set; }
    public int IdChefUser { get; set; }
}

namespace HttpServerWithGrpcClient.Controllers
{
    [Route("api/follow")]
    [ApiController]
    public class FollowController : Controller
    {
        [HttpPost]
        public HttpServerWothGrpcClient.Response Post([FromBody] RequestFollowData data)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var request = new HttpServerWothGrpcClient.RequestFollowUser();

            request.IdUser = data.IdUser;
            request.IdChefUser = data.IdChefUser;

            return client.FollowUser(request);
        }

        [HttpDelete]
        public HttpServerWothGrpcClient.Response Delete([FromBody] RequestFollowData data)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var request = new HttpServerWothGrpcClient.RequestFollowUser();

            request.IdUser = data.IdUser;
            request.IdChefUser = data.IdChefUser;

            return client.UnFollowUser(request);
        }
    }
}
