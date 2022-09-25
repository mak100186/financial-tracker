namespace Maxx.FinancialTracker.ServiceLayer.CollectionConfigs;

using Contracts;

using Repository.Configs;

public class CategoryCollectionConfigs : BaseCollectionConfigs, ICategoryCollectionConfigs
{
    public CategoryCollectionConfigs()
    {
        this.CollectionName = "categories";
    }
}