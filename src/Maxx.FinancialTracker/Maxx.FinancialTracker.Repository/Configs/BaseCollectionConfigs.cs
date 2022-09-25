namespace Maxx.FinancialTracker.Repository.Configs;

using Contracts;

public class BaseCollectionConfigs : IBaseCollectionConfigs
{
    public string CollectionName { get; set; } = null!;
}