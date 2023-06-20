using Moq;
using SmartMarket.Infrastructure;

namespace SmartMarket.Tests
{
    public class StockServiceTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task AddStockItemAsync_IsNullOrEmpty_Fails(string stockItem)
        {
            var stockSerializerMock = new Mock<StockSerializer>();
            var stockItemObject = stockSerializerMock.Object.Deserialize(stockItem);


            var result = stockItemObject.ProductName;

            
        }
    }
}
