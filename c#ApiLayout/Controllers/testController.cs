using scrumBackend.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace scrumBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class scrumController : ControllerBase
    {
        private readonly IMongoCollection<BsonDocument> _userCollection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<scrumController> _logger;

        public scrumController(IConfiguration configuration, IMongoClient mongoClient, ILogger<scrumController> logger)
        {
            _configuration = configuration;

            var client = mongoClient;
            _logger = logger;
            var userDatabase = client.GetDatabase("userDatabase");
            _userCollection = userDatabase.GetCollection<BsonDocument>("users");
        }

        [HttpPost("registerUser")]
        public IActionResult RegisterUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDocument = new BsonDocument
            {
                { "Name", userDto.Name },
                { "Email", userDto.Email },
                { "UserID", userDto.UserID },
                { "Date Created", DateTime.Now.ToString("MM-dd-yyyy HH:mm")},
            };

            _userCollection.InsertOne(userDocument);

            _logger.LogInformation("New user created. User ID: {UserID}, Name: {Name}, Email: {Email}", userDto.UserID, userDto.Name, userDto.Email);

            return Ok(new { message = "created user", name = userDto.Name, email = userDto.Email, userID = userDto.UserID});
        }

        [HttpGet("fetchUsers")]
        public IActionResult GetTickets()
        {
            var users = _userCollection.Find(new BsonDocument()).ToList();

            var userList = new List<object>();
            foreach (var user in users)
            {
                userList.Add(new
                {
                    Name = user.GetValue("Name").AsString,
                    Email = user.GetValue("Email").AsString,
                    UserID = user.GetValue("UserID").AsString,
                    DateCreated = DateTime.Parse(user.GetValue("Date Created").AsString),
                });
            }
            return Ok(userList);
        }
    }
}
