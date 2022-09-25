// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Maxx.FinancialTracker.ServiceLayer.Services;

using Contracts;

using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

using Repository.Contracts;
using Repository.Models.Models;

/// <summary>
///     Service to wrap communications with the repository
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly ILogger<TransactionService> _logger;
    private readonly IRepository<Transaction> _transactionRepository;

    public TransactionService(IRepository<Transaction> transactionRepository,
        IRepository<Category> categoryRepository,
        ICategoryCollectionConfigs categoryCollectionConfigs,
        ITransactionCollectionConfigs transactionCollectionConfigs,
        ILogger<TransactionService> logger)
    {
        this._transactionRepository = transactionRepository;
        this._categoryRepository = categoryRepository;
        this._logger = logger;

        this._categoryRepository.ConfigureCollection(categoryCollectionConfigs);
        this._transactionRepository.ConfigureCollection(transactionCollectionConfigs);
    }

    private async Task IncludeCategories(ICollection<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            await this.IncludeCategories(transaction);
        }
    }

    private async Task IncludeCategories(Transaction transaction)
    {
        if (!string.IsNullOrWhiteSpace(transaction.CategoryId) && transaction.Category == null)
        {
            transaction.Category = await this._categoryRepository.GetAsync(
                Builders<Category>.Filter.Eq(category => category.Id, transaction.CategoryId));
        }
    }

    public async Task<ICollection<Transaction>?> GetAll()
    {
        var transactions = await this._transactionRepository.GetAsync();

        await this.IncludeCategories(transactions);

        return transactions;
    }

    public async Task<Transaction?> Get(string id)
    {
        var filter = Builders<Transaction>.Filter.Eq(transaction => transaction.Id, id);

        var transaction = await this._transactionRepository.GetAsync(filter);

        await this.IncludeCategories(transaction);

        return transaction;
    }

    public async Task Add(Transaction transaction)
    {
        await this._transactionRepository.CreateAsync(transaction);
    }

    public async Task Update(Transaction transaction)
    {
        if (string.IsNullOrWhiteSpace(transaction.Id))
        {
            throw new ArgumentNullException(nameof(transaction.Id));
        }

        await this._transactionRepository.UpdateAsync(transaction.Id, transaction);
    }

    public async Task Remove(Transaction transaction)
    {
        await this._transactionRepository.RemoveAsync(transaction.Id);
    }

    public async Task<ICollection<Transaction>> GetAllInDateRange(DateTime startDate, DateTime endDate)
    {
        var filterBuilder = Builders<Transaction>.Filter;
        var filter = filterBuilder.Gte(transaction => transaction.Date, new BsonDateTime(startDate)) &
                     filterBuilder.Lte(transaction => transaction.Date, new BsonDateTime(endDate));

        var transactions = await this._transactionRepository.Collection.Find(filter).ToListAsync();

        await this.IncludeCategories(transactions);

        return transactions;
    }

    public async Task<ICollection<Transaction>> GetRecent(int count)
    {
        var transactions = await this._transactionRepository.Collection.Find(_ => true)
            .SortByDescending(transaction => transaction.Date)
            .Limit(count)
            .ToListAsync();

        await this.IncludeCategories(transactions);

        return transactions;
    }
}