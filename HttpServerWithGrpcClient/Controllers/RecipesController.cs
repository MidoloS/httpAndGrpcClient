using Grpc.Net.Client;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HttpServerWithGrpcClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        // GET: api/<RecipesController>
        [HttpGet]
        public HttpServerWothGrpcClient.ResponseRecipes Get()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var empty = new HttpServerWothGrpcClient.Empty();

            var reply = client.GetAllRecipes(empty);
            return reply;
        }

        // GET api/<RecipesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RecipesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RecipesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RecipesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
