namespace Maxx.FinancialTracker.Repository.Repository;

using Contracts;

using Microsoft.Extensions.Logging;

using Models.Contracts;

using MongoDB.Driver;

/// <summary>
///     Generic repository that represents the collection and offers CRUD operations on it.
/// </summary>
/// <typeparam name="TCollectionItem"></typeparam>
public class Repository<TCollectionItem> : IRepository<TCollectionItem>
    where TCollectionItem : ICollectionItem
{
    private readonly IMongoDbConnection _connection;
    private readonly ILogger<Repository<TCollectionItem>> _logger;

    public Repository(IMongoDbConnection connection,
        ILogger<Repository<TCollectionItem>> logger)
    {
        this._logger = logger;
        this._connection = connection;
    }

    /// <summary>
    ///     Must be called after constructor to connect the repository to the collection
    /// </summary>
    /// <param name="collectionConfigs"></param>
    public void ConfigureCollection(IBaseCollectionConfigs collectionConfigs)
    {
        this.Collection = this._connection.Database.GetCollection<TCollectionItem>(
            collectionConfigs.CollectionName);
    }

    /// <summary>
    ///     Reference to the actual collection in the database
    /// </summary>
    public IMongoCollection<TCollectionItem> Collection { get; private set; } = null!;

    /// <inheritdoc />
    public async Task<TCollectionItem> GetAsync(FilterDefinition<TCollectionItem> filter)
    {
        this._logger.LogInformation("GetAsync() called");

        return await this.Collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<List<TCollectionItem>> GetAsync()
    {
        this._logger.LogInformation("GetAsync() called");

        return await this.Collection.Find(_ => true).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TCollectionItem?> GetAsync(string id)
    {
        this._logger.LogInformation("GetAsync({id}) called", id);

        return await this.Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TCollectionItem>?> GetManyAsync(IEnumerable<string> ids)
    {
        this._logger.LogInformation("GetManyAsync({id}) called", string.Join(",", ids));

        var filter = Builders<TCollectionItem>.Filter.In(definition => definition.Id, ids);

        return await this.Collection.Find(filter).ToListAsync();
    }

    /// <inheritdoc />
    public async Task CreateAsync(TCollectionItem item)
    {
        this._logger.LogInformation("CreateAsync() called");
        await this.Collection.InsertOneAsync(item);
    }

    public async Task CreateManyAsync(IEnumerable<TCollectionItem> items)
    {
        this._logger.LogInformation("CreateManyAsync() called");
        await this.Collection.InsertManyAsync(items);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(string id, TCollectionItem updatedItem)
    {
        this._logger.LogInformation("UpdateAsync() called");
        await this.Collection.ReplaceOneAsync(x => x.Id == id, updatedItem);
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string id)
    {
        this._logger.LogInformation("RemoveAsync() called");
        await this.Collection.DeleteOneAsync(x => x.Id == id);
    }
}