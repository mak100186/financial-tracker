namespace Maxx.FinancialTracker.ServiceLayer.Services;

using Contracts;

using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using Repository.Contracts;
using Repository.Models.Models;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly ILogger<TransactionService> _logger;

    public CategoryService(IRepository<Transaction> transactionRepository,
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

    public async Task<ICollection<Category>?> GetAll()
    {
        return await this._categoryRepository.GetAsync();
    }

    public async Task<Category?> Get(string id)
    {
        var filter = Builders<Category>.Filter.Eq(category => category.Id, id);

        return await this._categoryRepository.GetAsync(filter);
    }

    public async Task Remove(Category category)
    {
        await this._categoryRepository.RemoveAsync(category.Id);
    }

    public async Task Update(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Id))
        {
            throw new ArgumentNullException(nameof(category.Id));
        }

        await this._categoryRepository.UpdateAsync(category.Id, category);
    }

    public async Task Add(Category category)
    {
        await this._categoryRepository.CreateAsync(category);
    }
}