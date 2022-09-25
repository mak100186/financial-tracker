namespace Maxx.FinancialTracker.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

using Repository.Models.Models;

using ServiceLayer.Contracts;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ITransactionService _transactionService;

    public CategoryController(ITransactionService transactionService,
        ICategoryService categoryService)
    {
        this._transactionService = transactionService;
        this._categoryService = categoryService;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        var categories = await this._categoryService.GetAll();

        return categories != null ? this.View(categories.ToList()) : this.Problem("Entity set 'Categories' is null.");
    }


    // GET: Category/AddOrEdit
    public async Task<IActionResult> AddOrEdit(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return this.View(new Category());
        }

        return this.View(await this._categoryService.Get(id));
    }

    // POST: Category/AddOrEdit
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEdit([Bind("Id,Title,Icon,Type")] Category category)
    {
        //if (this.ModelState.IsValid)
        {
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                await this._categoryService.Add(category);
            }
            else
            {
                await this._categoryService.Update(category);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        return this.View(category);
    }


    // POST: Category/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var categories = await this._categoryService.GetAll();
        if (categories == null)
        {
            return this.Problem("Entity set 'Categories'  is null.");
        }

        var category = await this._categoryService.Get(id);
        if (category != null)
        {
            await this._categoryService.Remove(category);
        }

        return this.RedirectToAction(nameof(this.Index));
    }
}