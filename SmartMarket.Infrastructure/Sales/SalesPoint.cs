using SmartMarket.Core.Interfaces;
using SmartMarket.Core;
using System.Net;

namespace SmartMarket.Infrastructure;

public class SalesPoint
{
    private readonly Dictionary<string, int> _productsInCart;
    private readonly List<StockItemProxy> _stock;
    private readonly IDateTimeNow _dateTime;

    public SalesPoint(List<StockItemProxy> stock, IDateTimeNow date)
    {
        _stock = stock;
        _dateTime = date;
        _productsInCart = new Dictionary<string, int>();
    }
    
    public void ScanItem(string productName)
    {
        var stockItem = _stock.FirstOrDefault(x => x.ProductName == productName);
        if (stockItem is null)
        {
            throw new ArgumentException($"Product {productName} not found in stock");
        }

        if (_productsInCart.TryGetValue(productName, out var quantity))
        {
            _productsInCart[productName] = quantity + 1;
        }
        else
        {
            _productsInCart.Add(productName, 1);
        }
    }

    public Dictionary<string, decimal> GetTotals()
    {
        var totals = new Dictionary<string, decimal>();
        foreach (var (product, quantity) in _productsInCart)
        {
            var stockItem = _stock.First(x => x.ProductName == product);
            
            var total = GetTotalByMemberShip(stockItem, quantity);

            if (_dateTime.DateNow() is DayOfWeek.Monday or DayOfWeek.Tuesday)
            {
                total -= total * 0.05m;
            }

            if (_dateTime.DateNow() is DayOfWeek.Saturday && stockItem.ProductName[0] == 's' || stockItem.ProductName[0] == 'S')
            {
                total -= total * 0.10m;
            }

            totals.Add(product, total);
        }
        return totals;
    }

    public decimal GetTotalByMemberShip(StockItemProxy itemProxy, int quantity)
    {
        var total = itemProxy.Price * quantity;

        if (itemProxy.MembershipDeal is not null)
        {
            var numberOfDeals = quantity / itemProxy.MembershipDeal.Quantity;
            var remainder = quantity % itemProxy.MembershipDeal.Quantity;
            return total = numberOfDeals * itemProxy.MembershipDeal.Price + remainder * itemProxy.Price;
        }

        return total;
    }
}