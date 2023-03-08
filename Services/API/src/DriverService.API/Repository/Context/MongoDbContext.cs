using DriverService.API.Domain.Utils;
using MongoDB.Driver;
using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Repository.Context
{
    public class MongoDbContext
    {
        private IMongoDatabase _database { get; }

        public MongoDbContext(ISettings appSettings)
        {
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(appSettings.MONGODB_CONNECTION_STRING));
                if (appSettings.MONGODB_IS_SSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }
                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(appSettings.MONGODB_DATABASE);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot connect with the server", ex);
            }
        }

        public IMongoCollection<Route> Routes
        {
            get
            {
                return _database.GetCollection<Route>("routes");
            }
        }
    }
}
