namespace Maxx.FinancialTracker.Repository.Contracts;

public interface IMongoDbCredentials
{
    string Username { get; set; }
    string Password { get; set; }
}