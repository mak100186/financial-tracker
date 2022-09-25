namespace Maxx.FinancialTracker.ServiceLayer.CollectionConfigs;

using Contracts;

using Repository.Configs;

public class TransactionCollectionConfigs : BaseCollectionConfigs, ITransactionCollectionConfigs
{
    public TransactionCollectionConfigs()
    {
        this.CollectionName = "transactions";
    }
}