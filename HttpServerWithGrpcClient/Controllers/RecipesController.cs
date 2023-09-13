using Grpc.Net.Client;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

public class PhotoData
{
    public int Id { get; set; }
    public string Url { get; set; }
}

public class SteptData
{
    public int Id { get; set; }
    public string Description { get; set; }
}

public class IngredientData
{
    public int Id { get; set; }
    public string Name { get; set; }
    
}

public class RecipeData
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public int IdCategory { get; set; }

    public List<PhotoData> Photos { get; set; }
    public List<SteptData> Stepts { get; set; }
    public List<IngredientData> Ingredients { get; set; }


   

}

namespace HttpServerWithGrpcClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
    [HttpPost("favorites/{recipe_id}")]
        public HttpServerWothGrpcClient.Response likeRecipe(int recipe_id, [FromBody] int idUser)
        {
            try
            {
                Debug.WriteLine("like");
                Debug.WriteLine("id");
                Debug.WriteLine($"{recipe_id}");
                Debug.WriteLine("user");
                Debug.WriteLine($"{idUser}");
                using var channel = GrpcChannel.ForAddress("http://localhost:50051/");
                Debug.WriteLine(1);
                var client = new ChefEnCasa.ChefEnCasaClient(channel);
                Debug.WriteLine(2);
                var body = new HttpServerWothGrpcClient.RequestReciepeToFavorites
                {
                    IdReciepe = recipe_id,
                    IdUser = idUser
                };

                Debug.WriteLine(3);

                return client.AddReciepeToFavorites(body);
            } catch (Exception ex)
            {
                Debug.WriteLine("---------------------------");
                Debug.WriteLine(ex.ToString());
                


                throw new Exception();
            }
        }

        [HttpDelete("favorites/{recipe_id}")]
        public HttpServerWothGrpcClient.Response dislikeRecipe(int recipe_id, [FromBody] int idUser)
        {
            Debug.WriteLine("dislike");
            Debug.WriteLine("id");
            Debug.WriteLine($"{recipe_id}");
            Debug.WriteLine("user");
            Debug.WriteLine($"{idUser}");
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var body = new HttpServerWothGrpcClient.RequestReciepeToFavorites
            {
                IdReciepe = recipe_id,
                IdUser = idUser
            };

            return client.RemoveReciepeToFavorites(body);
        }
        // POST api/<RecipesController>
            [HttpPost]
public HttpServerWothGrpcClient.Response Post([FromBody] RecipeData reciepeData)
         {
           Debug.WriteLine("debug information");

           using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
           {
               Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
           });

           var client = new ChefEnCasa.ChefEnCasaClient(channel);
           var request = new HttpServerWothGrpcClient.RequestCreateReciepe();
           request.Title = reciepeData.Title;
           request.Description = reciepeData.Description;
           request.PrepatarionTimeMinutes = reciepeData.PreparationTimeMinutes;
           var category = new HttpServerWothGrpcClient.Category();
           category.Id = reciepeData.IdCategory;
           request.Category = category;

           foreach (PhotoData photo in reciepeData.Photos)
           {
               var p = new HttpServerWothGrpcClient.Photo();
               p.Id = photo.Id;
               p.Url = photo.Url;
               request.Photos.Add(p);
           }

           foreach (SteptData stept in reciepeData.Stepts)
           {
               var s = new HttpServerWothGrpcClient.Stept();
               s.Id = stept.Id;
               s.Description = stept.Description;
               request.Stepts.Add(s);
           }

           foreach (IngredientData ingredient in reciepeData.Ingredients)
           {
               var i = new HttpServerWothGrpcClient.Ingredient();
               i.Id = ingredient.Id;
               i.Name = ingredient.Name;
               request.Ingredients.Add(i);
           }


           var reply = client.CreateRecipe(request);

           return reply;
         }

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
        public HttpServerWothGrpcClient.ResponseRecipe Get(int id)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);
            var request = new HttpServerWothGrpcClient.RequestById();
            request.Id = id;
            return client.GetRecipeById(request);
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
        public HttpServerWothGrpcClient.Response Put(int id,[FromBody] RecipeData reciepeData)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });

            var client = new ChefEnCasa.ChefEnCasaClient(channel);
            var request = new HttpServerWothGrpcClient.RequestUpdateReciepe();
            request.IdReciepe = id;
            request.Title = reciepeData.Title;
            request.Description = reciepeData.Description;
            request.PrepatarionTimeMinutes = reciepeData.PreparationTimeMinutes;
            var category = new HttpServerWothGrpcClient.Category();
            category.Id = reciepeData.IdCategory;
            request.Category = category;
            
            foreach (PhotoData photo in reciepeData.Photos)
            {
                var p = new HttpServerWothGrpcClient.Photo();
                p.Id = photo.Id;
                p.Url = photo.Url;
                request.Photos.Add(p);
            }

            foreach (SteptData stept in reciepeData.Stepts)
            {
                var s = new HttpServerWothGrpcClient.Stept();
                s.Id = stept.Id;
                s.Description = stept.Description;
                request.Stepts.Add(s);
            }

            foreach(IngredientData ingredient in reciepeData.Ingredients)
            {
                var i = new HttpServerWothGrpcClient.Ingredient();
                i.Id = ingredient.Id;
                i.Name = ingredient.Name;
                request.Ingredients.Add(i);
            }


            var reply = client.UpdateReciepe(request);

            return reply;
        }

        // DELETE api/<RecipesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
