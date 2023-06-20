using Moq;
using SmartMarket.Infrastructure;
using SmartMarket.Core.Interfaces;
using SmartMarket.Core;

namespace SmartMarket.Tests;

public class SalesPointTests
{
    /*
     * Escanear un producto con una sola cantidad y sin ofertas de membresía retorna precio por cantidad.
     */
    [Theory]
    [InlineData(DayOfWeek.Wednesday)]
    public void ScanningOneQuantityItem_NoMembershipDeals_ReturnsPricexQuantity(DayOfWeek date)
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider"
            }
        };
        var _date = new Mock<IDateTimeNow>();
        _date.Setup(x => x.DateNow())
            .Returns(date);

        var salesPoint = new SalesPoint(stock, _date.Object);

        // Act
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();
        
        //Assert
        Assert.Equal(1.23m, totals["Milk"]);
    }

    /*
    * Escanear un producto mas de una vez y sin ofertas de membresía retorna precio por cantidad.
    */
    [Theory]
    [InlineData(DayOfWeek.Thursday)]
    public void ScanningOneItemMultipleTimes_NoMembershipDeals_ReturnsPricexQuantity(DayOfWeek date)
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider"
            }
        };
        var _date = new Mock<IDateTimeNow>();
        _date.Setup(x => x.DateNow())
            .Returns(date);

        var salesPoint = new SalesPoint(stock, _date.Object);

        // Act
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();

        //Assert
        const decimal expectedTotal = 1.23m * 3;
        Assert.Equal(expectedTotal, totals["Milk"]);
    }

    /*
    * Escanear un producto mas de una vez con ofertas de membresía retorna precio de membresía.
    */
    [Theory]
    [InlineData(DayOfWeek.Wednesday)]
    public void ScanningAnItem_WithMembershipDeal_ReturnsMembershipDealPrice(DayOfWeek date)
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider",
                MembershipDeal = new MembershipDeal
                {
                    Price = 2.00m,
                    Quantity = 3,
                    Product = "Milk"
                }
            }
        };
        var _date = new Mock<IDateTimeNow>();
        _date.Setup(x => x.DateNow())
            .Returns(date);

        var salesPoint = new SalesPoint(stock, _date.Object);

        // Act
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();

        //Assert
        Assert.Equal(2.00m, totals["Milk"]);
    }

    /*
    * Escanear un producto mas de una vez con ofertas de membresía y mas cantidad retorna precio de membresía mas precio por cantidad.
    */
    [Theory]
    [InlineData(DayOfWeek.Wednesday)]
    public void ScanningAnItem_WithMembershipDealAndMoreQuantity_ReturnsMembershipDealPricePlusPriceByQuantity(DayOfWeek date)
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider",
                MembershipDeal = new MembershipDeal
                {
                    Price = 2.00m,
                    Quantity = 3,
                    Product = "Milk"
                }
            }
        };
        var _date = new Mock<IDateTimeNow>();
        _date.Setup(x => x.DateNow())
            .Returns(date);

        var salesPoint = new SalesPoint(stock, _date.Object);

        // Act
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();

        //Assert
        Assert.Equal(2.00m + 1.23m, totals["Milk"]);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    public void ScanningAnItem_OnMondayOrTuesday_ReturnsFivePercentDiscount(DayOfWeek date)
    {
        //Act
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider",
                MembershipDeal = new MembershipDeal
                {
                    Price = 2.00m,
                    Quantity = 3,
                    Product = "Milk"
                }
            }
        };

        var _date = new Mock<IDateTimeNow>();
        _date.Setup(x => x.DateNow())
            .Returns(date);

        var salesPoint = new SalesPoint(stock, _date.Object);

        // Act
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();

        //Assert
        Assert.Equal(1.23m * 0.95m, totals["Milk"]);
    }
}