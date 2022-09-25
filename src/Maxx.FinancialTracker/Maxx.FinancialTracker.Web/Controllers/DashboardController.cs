namespace Maxx.FinancialTracker.Web.Controllers;

using System.Globalization;

using Microsoft.AspNetCore.Mvc;

using ServiceLayer.Contracts;
using ServiceLayer.Extensions;

public class DashboardController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ITransactionService _transactionService;

    public DashboardController(ITransactionService transactionService,
        ICategoryService categoryService)
    {
        this._transactionService = transactionService;
        this._categoryService = categoryService;
    }

    public async Task<ActionResult> Index()
    {
        //This month
        var date = DateTime.Now;
        var startDate = new DateTime(date.Year, date.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var daysInMonth = endDate.Day;

        this.ViewBag.MonthName = startDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("en"));

        var selectedTransactions = (await this._transactionService.GetAllInDateRange(startDate, endDate))
            .ToList();

        //Total Income
        var totalIncome = selectedTransactions
            .Where(i => i.Category.Type == "Income")
            .Sum(j => j.Amount);
        this.ViewBag.TotalIncome = totalIncome.ToString("C0");

        //Total Expense
        var totalExpense = selectedTransactions
            .Where(i => i.Category.Type == "Expense")
            .Sum(j => j.Amount);
        this.ViewBag.TotalExpense = totalExpense.ToString("C0");

        //Balance
        var balance = totalIncome - totalExpense;
        var culture = CultureInfo.CreateSpecificCulture("en-US");
        culture.NumberFormat.CurrencyNegativePattern = 1;
        this.ViewBag.Balance = string.Format(culture, "{0:C0}", balance);

        //Doughnut Chart - Expense By Category
        this.ViewBag.DoughnutChartData = selectedTransactions
            .Where(i => i.Category.Type == "Expense")
            .GroupBy(j => j.Category.Id)
            .Select(k => new
            {
                categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                amount = k.Sum(j => j.Amount),
                formattedAmount = k.Sum(j => j.Amount).ToString("C0")
            })
            .OrderByDescending(l => l.amount)
            .ToList();

        //Spline Chart - Income vs Expense

        //Income
        var incomeSummary = selectedTransactions
            .Where(i => i.Category.Type == "Income")
            .GroupBy(j => j.Date)
            .Select(k => new SplineChartData
            {
                day = k.First().Date.ToString("dd"),
                income = k.Sum(l => l.Amount)
            })
            .ToList();

        //Expense
        var expenseSummary = selectedTransactions
            .Where(i => i.Category.Type == "Expense")
            .GroupBy(j => j.Date)
            .Select(k => new SplineChartData
            {
                day = k.First().Date.ToString("dd"),
                expense = k.Sum(l => l.Amount)
            })
            .ToList();

        //Combine Income & Expense
        var datesOfThisMonth = Enumerable.Range(0, daysInMonth)
            .Select(i => startDate.AddDays(i).ToString("dd"))
            .ToArray();

        this.ViewBag.SplineChartData = from day in datesOfThisMonth
            join income in incomeSummary on day equals income.day into dayIncomeJoined
            from income in dayIncomeJoined.DefaultIfEmpty()
            join expense in expenseSummary on day equals expense.day into expenseJoined
            from expense in expenseJoined.DefaultIfEmpty()
            select new
            {
                day,
                income = income?.income ?? 0,
                expense = expense?.expense ?? 0
            };

        //Recent Transactions
        this.ViewBag.RecentTransactions = (await this._transactionService.GetRecent(9))
            .ToList();

        return this.View();
    }
}

public class SplineChartData
{
    public string day;
    public int expense;
    public int income;
}