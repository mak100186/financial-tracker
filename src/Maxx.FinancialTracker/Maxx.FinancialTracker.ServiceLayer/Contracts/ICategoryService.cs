namespace Maxx.FinancialTracker.ServiceLayer.Contracts;

using Repository.Models.Models;

public interface ICategoryService
{
    Task<ICollection<Category>?> GetAll();
    Task<Category?> Get(string id);
    Task Remove(Category category);
    Task Update(Category category);
    Task Add(Category category);
}