using SmartMarket.Core;
using SmartMarket.Core.Interfaces;
using SmartMarket.Infrastructure.Rules;

namespace SmartMarket.Infrastructure;

public class StockService : IStockService
{
    public async Task<bool> AddStockItemAsync(string stockItem)
    {
        var stockSerializer = new StockSerializer();
        var stockItemObject = stockSerializer.Deserialize(stockItem);

        if (string.IsNullOrEmpty(stockItemObject.ProductName))
        {
            return false;
        }

        if (stockItemObject.Price <=0)
        {
            return false;
        }

        var isExpired = ExpirationRule.ExpirationConditions(stockItemObject);
        if (!isExpired)
        {
            return false;
        }

        using (var providerManagementService = new ProviderManagementService())
        {
            var provider = await providerManagementService.GetFromApiByIdAsync(stockItemObject.ProviderId);
            if (provider is null)
            {
                SmartMarketDataAccess.AddProvider(stockItemObject.ProviderId, stockItemObject.ProviderName);
            }
        }
        SmartMarketDataAccess.AddStockItem(stockItemObject);
        return true;
    }

}