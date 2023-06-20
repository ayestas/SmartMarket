namespace SmartMarket.Infrastructure.Rules
{
    public abstract class RuleBase
    {
        public abstract Dictionary<string, decimal> GetTotals();

        protected virtual void ScanItem(string productName)
        {

        }

    }
}
