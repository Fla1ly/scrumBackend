using scrumBackend.Utilities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace scrumBacked.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class scrumController : ControllerBase
    {
        private readonly IMongoCollection<BsonDocument> _userCollection;
        private readonly IConfiguration _configuration;

        public scrumController(IConfiguration configuration, IMongoClient mongoClient)
        {
            _configuration = configuration;

            var client = mongoClient;
            var userDatabase = client.GetDatabase("userDatabase");
            _userCollection = userDatabase.GetCollection<BsonDocument>("users");
        }

        [HttpPost("registerUser")]
        public IActionResult dtoEndpoint([FromBody] UserDto userForm)
        {
            string username = userForm.name;
            string email = userForm.email;
            Log.LogEvent(_userCollection, username, email);
            return Ok(username);
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
                    Name = user.GetValue("username").AsString,
                    Email = user.GetValue("email").AsString,
                });
            }
            return Ok(userList);
        }
    }
}
