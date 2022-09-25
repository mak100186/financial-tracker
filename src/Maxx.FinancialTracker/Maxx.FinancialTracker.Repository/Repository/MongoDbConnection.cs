namespace Maxx.FinancialTracker.Repository.Repository;

using Configs;

using Contracts;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

public class MongoDbConnection : IMongoDbConnection
{
    public MongoDbConnection(IOptions<DatabaseConfigs> databaseConfigs)
    {
        var mongoClient = new MongoClient(
            databaseConfigs.Value.ConnectionString);

        this.Database = mongoClient.GetDatabase(
            databaseConfigs.Value.DatabaseName);
    }

    public IMongoDatabase Database { get; }
}