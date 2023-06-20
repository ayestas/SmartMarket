using Moq;
using SmartMarket.Core;
using SmartMarket.Infrastructure;
using System.Diagnostics;

namespace SmartMarket.Tests
{
    public class StockServiceTests
    {
        [Theory]
        [InlineData("")]
        public async Task AddStockItemAsync_IsNullOrEmpty_Fails(string stockItem)
        {
            var stock = new StockItem
            {
                ProductName = stockItem,
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider"
            };

            var service = new StockService();
            var result = await service.AddStockItemAsync(stock.ToString());

            Assert.True(result);
        }

        [Theory]
        [InlineData(0, "Apple")]
        public async Task AddStockItemAsync_PriceZeroNegative_Fails(int price, string stockItem)
        {
            var stock = new StockItem
            {
                ProductName = stockItem,
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider"
            };

            var service = new StockService();
            var result = await service.AddStockItemAsync(stock.ToString());

            Assert.Equal(price, stock.Price);
        }


    }
}
