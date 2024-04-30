using MongoDB.Bson;
using MongoDB.Driver;

namespace scrumBackend.Utilities
{
    public class Log
    {
        public static void LogEvent(IMongoCollection<BsonDocument> logCollection, string username, string email)
        {
            var log = new BsonDocument
         {
             { "username", username },
             { "email", email}
         };
            logCollection.InsertOne(log);
        }
    }
}
