using Grpc.Net.Client;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;

namespace HttpServerWithGrpcClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}