namespace Maxx.FinancialTracker.Repository.Contracts;

public interface IDatabaseConfigs
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}