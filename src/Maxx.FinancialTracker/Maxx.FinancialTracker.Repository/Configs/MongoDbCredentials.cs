namespace Maxx.FinancialTracker.Repository.Configs;

using Contracts;

public class MongoDbCredentials : IMongoDbCredentials
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}