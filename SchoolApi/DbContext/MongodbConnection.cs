using MongoDB.Driver;

namespace SchoolApi.DbContext
{
    public class MongodbConnection
    {
        private readonly IMongoDatabase _database;

        public MongodbConnection()
        {
            // Direct MongoDB connection string and DB name
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "school";

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
