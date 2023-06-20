using SmartMarket.Core;

namespace SmartMarket.Infrastructure
{
    public class StockItemProxy
    {
        private readonly StockItem _stockItem;

        public StockItemProxy(StockItem stockItem)
        {
            _stockItem = stockItem;
        }

        public string ProductName => _stockItem.ProductName;
        public decimal Price => _stockItem.Price;
        public DateOnly ProducedOn => _stockItem.ProducedOn;
        public Guid ProviderId => _stockItem.ProviderId;
        public string ProviderName => _stockItem.ProviderName;
        public MembershipDeal? MembershipDeal => _stockItem.MembershipDeal;
        public bool IsCloseToExpirationDate => _stockItem.IsCloseToExpirationDate;



    }
}
