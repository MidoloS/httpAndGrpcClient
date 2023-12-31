﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using HttpServerWithGrpcClient;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

public class Ingredient
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

namespace HttpServerWithGrpcClient.Controllers
{
    [Route("api/ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {



        // GET: api/<ValuesController>
        [HttpGet]
        public HttpServerWothGrpcClient.ResponseIngredients Get()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var empty = new HttpServerWothGrpcClient.Empty();

            var reply = client.GetAllIngredients(empty);



            return reply;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
