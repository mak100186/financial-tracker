namespace Maxx.FinancialTracker.Web.Controllers;

using System.Threading.Tasks;

using Maxx.FinancialTracker.Repository.Models.Models;

using Microsoft.AspNetCore.Mvc;

using ServiceLayer.Contracts;

public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly ICategoryService _categoryService;

    public TransactionController(ITransactionService transactionService, 
        ICategoryService categoryService)
    {
        this._transactionService = transactionService;
        this._categoryService = categoryService;
    }

    // GET: Transaction
    public async Task<IActionResult> Index()
    {
        var transactions = await this._transactionService.GetAll();
        
        return View(transactions.ToList());
    }

    // GET: Transaction/AddOrEdit
    public async Task<IActionResult> AddOrEdit(string id)
    {
        this.PopulateCategories();

        return string.IsNullOrWhiteSpace(id) ? this.View(new Transaction()) : this.View(await this._transactionService.Get(id));
    }

    // POST: Transaction/AddOrEdit
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEdit([Bind("Id,CategoryId,Amount,Note,Date")] Transaction transaction)
    {
        //if (this.ModelState.IsValid)
        {
            if (string.IsNullOrWhiteSpace(transaction.Id))
                await this._transactionService.Add(transaction);
            else
                await this._transactionService.Update(transaction);
            
            return this.RedirectToAction(nameof(this.Index));
        }
        await this.PopulateCategories();

        return View(transaction);
    }

    // POST: Transaction/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (await this._transactionService.GetAll() == null)
        {
            return this.Problem("Entity set 'Transactions'  is null.");
        }

        var transaction = await this._transactionService.Get(id);
        if (transaction != null)
        {
            this._transactionService.Remove(transaction);
        }
        
        return this.RedirectToAction(nameof(this.Index));
    }


    [NonAction]
    private async Task PopulateCategories()
    {
        var categoryCollection = (await this._categoryService.GetAll()).ToList();

        var defaultCategory = new Category { Title = "Choose a Category" };
        categoryCollection.Insert(0, defaultCategory);
        this.ViewBag.Categories = categoryCollection;
    }
}