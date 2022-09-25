namespace Maxx.FinancialTracker.Web.Extensions;

using Repository.Configs;
using Repository.Contracts;
using Repository.Models.Models;
using Repository.Repository;

using ServiceLayer.CollectionConfigs;
using ServiceLayer.Contracts;
using ServiceLayer.Services;

public static class Extensions
{
    public static void ConfigureMongoDb(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseConfigs>(
            builder.Configuration.GetSection("Database"));

        var dbCredentials = builder.Configuration.GetSection("DatabaseCredentials");

        builder.Services.AddSingleton<IMongoDbCredentials>(
            new MongoDbCredentials
            {
                Username = dbCredentials["DatabaseCredentials:Username"],
                Password = dbCredentials["DatabaseCredentials:Password"]
            });

        builder.Services.AddSingleton<IMongoDbConnection, MongoDbConnection>();
        builder.Services.AddSingleton<ITransactionCollectionConfigs, TransactionCollectionConfigs>();
        builder.Services.AddSingleton<ICategoryCollectionConfigs, CategoryCollectionConfigs>();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITransactionService, TransactionService>();
        builder.Services.AddSingleton<ICategoryService, CategoryService>();
        builder.Services.AddSingleton<IRepository<Transaction>, Repository<Transaction>>();
        builder.Services.AddSingleton<IRepository<Category>, Repository<Category>>();
    }
}