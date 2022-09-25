namespace Maxx.FinancialTracker.Repository.Contracts;

using MongoDB.Driver;

public interface IMongoDbConnection
{
    IMongoDatabase Database { get; }
}