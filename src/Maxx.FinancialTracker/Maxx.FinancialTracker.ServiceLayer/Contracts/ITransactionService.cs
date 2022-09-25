namespace Maxx.FinancialTracker.ServiceLayer.Contracts;

using Repository.Models.Models;

public interface ITransactionService
{
    Task<ICollection<Transaction>?> GetAll();
    Task<Transaction?> Get(string id);
    Task Add(Transaction transaction);
    Task Update(Transaction transaction);
    Task Remove(Transaction transaction);
    Task<ICollection<Transaction>> GetAllInDateRange(DateTime startDate, DateTime endDate);
    Task<ICollection<Transaction>> GetRecent(int count);
}