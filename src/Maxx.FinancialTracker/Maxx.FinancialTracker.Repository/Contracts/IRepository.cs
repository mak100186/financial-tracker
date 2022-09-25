namespace Maxx.FinancialTracker.Repository.Contracts;

using Maxx.FinancialTracker.Repository.Models.Contracts;

using MongoDB.Driver;

public interface IRepository<TCollectionItem> where TCollectionItem : ICollectionItem
{
    void ConfigureCollection(IBaseCollectionConfigs collectionConfigs);

    IMongoCollection<TCollectionItem> Collection { get; }

    Task<TCollectionItem> GetAsync(FilterDefinition<TCollectionItem> filter);

    Task<List<TCollectionItem>> GetAsync();

    Task<TCollectionItem?> GetAsync(string id);

    Task<IEnumerable<TCollectionItem>?> GetManyAsync(IEnumerable<string> ids);

    Task CreateAsync(TCollectionItem item);

    Task CreateManyAsync(IEnumerable<TCollectionItem> items);

    Task UpdateAsync(string id, TCollectionItem updatedItem);

    Task RemoveAsync(string id);
}