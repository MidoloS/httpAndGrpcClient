// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
 
﻿using Grpc.Net.Client;
using HttpServerWothGrpcClient;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class UserData
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

namespace HttpServerWithGrpcClient.Controllers
{


    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {

        public class LoginRequest
        {
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class UserRequest
        {
            public int IdUser { get; set; }
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

        [HttpGet("{id}")]
        public HttpServerWothGrpcClient.ResponseUser Get(int id)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);



            var body = new HttpServerWothGrpcClient.RequestByUser
            {
                Id = id
            };

            return client.GetUserById(body);
        }

        [HttpGet("top")]
        public HttpServerWothGrpcClient.ResponseUsers GetTop()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);


            var empty = new HttpServerWothGrpcClient.Empty();

            return client.GetTop10Users(empty);
        }

        [HttpPost("follow/{chefId}")]
        public HttpServerWothGrpcClient.Response Follow(int chefId, [FromBody] int userId)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);



            var body = new RequestFollowUser
            {
                IdChefUser = chefId,
                IdUser = userId
            };

            Debug.WriteLine("body");
            Debug.WriteLine(body);

            return client.FollowUser(body);
        }

        [HttpPost("unfollow/{chefId}")]
        public HttpServerWothGrpcClient.Response Unfollow(int chefId, [FromBody] int userId)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051/");

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var body = new RequestFollowUser
            {
                IdChefUser = chefId,
                IdUser = userId
            };

            Debug.WriteLine("body");
            Debug.WriteLine(body);

            return client.UnFollowUser(body);
        }


        [HttpPost]
        public dynamic user([FromBody] UserData userData)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:50051", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure // You might need to replace this with secure credentials
            });

            var client = new ChefEnCasa.ChefEnCasaClient(channel);

            var request = new RequestRegisterUser();
            request.Name = userData.Name;
            request.LastName = userData.LastName;
            request.Email = userData.Email;
            request.UserName = userData.UserName;
            request.Password = userData.Password;

            var reply = client.RegisterUser(request);

            if (reply.Message == "-1")
            {
                return new
                {
                    succes = false,
                    message = "No se creo el usuario",
                    idUsuario = ""
                };

            }
            else
            {
                return new
                {
                    succes = true,
                    message = "Usuario creado",
                    idUsuario = reply.Message

                };
            }
        }

    }
}

