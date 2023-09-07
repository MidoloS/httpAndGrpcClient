using Grpc.Net.Client;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HttpServerWithGrpcClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // HttpServerWothGrpcClient.ResponseUser

        public class LoginRequest
        {
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        [HttpPost("login")]
        public HttpServerWothGrpcClient.ResponseUser Login([FromBody] LoginRequest loginRequest)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            string password = loginRequest.Password;
            string username = loginRequest.UserName;

            var body = new HttpServerWothGrpcClient.RequestGetUser
            {
                Password = password,
                UserName = username,
            };

            return client.GetUser(body);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
