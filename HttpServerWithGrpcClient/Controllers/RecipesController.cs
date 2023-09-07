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
        public HttpServerWothGrpcClient.ResponseRecipies Get([FromQuery] string title = ".", [FromQuery] int prepTime = 120, [FromQuery] int categoryId = 0)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            Console.WriteLine(title);

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var category = new HttpServerWothGrpcClient.Category {
                Id = categoryId,
                Name = "Category",
            };

            var ingredients = new HttpServerWothGrpcClient.Ingredient
            {
                Id = 2,
                Name = "Ing2 "
            };

            var body = new HttpServerWothGrpcClient.RequestGetRecipiesByFilters
            {
                Title = title,
                PrepatarionTimeMinutesMax = prepTime,
                Category = category,
                PrepatarionTimeMinutesMin = 0,
            };

            var reply = client.GetRecipiesByFilters(body);
            return reply;
        }

        // GET api/<RecipesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("favorites/{user_id}")]
        public HttpServerWothGrpcClient.ResponseRecipies GetFav(int id)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var body = new HttpServerWothGrpcClient.RequestByUser {
                Id = 1,
            };

            return client.GetAllFavoritesReciepes(body);


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
