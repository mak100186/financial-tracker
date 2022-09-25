namespace Maxx.FinancialTracker.Repository.Configs;

using Contracts;

public class DatabaseConfigs : IDatabaseConfigs
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;

}