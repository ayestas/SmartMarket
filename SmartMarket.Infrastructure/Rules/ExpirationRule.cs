using SmartMarket.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Infrastructure.Rules
{
    public class ExpirationRule
    {
        public static bool ExpirationConditions(StockItem stockItem)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var currentAge = now.DayNumber - stockItem.ProducedOn.DayNumber;

            switch (currentAge)
            {
                case > 30:
                    return false;
                case > 15:
                case > 7 when stockItem.MembershipDeal is not null:
                    stockItem.IsCloseToExpirationDate = true;
                    break;
                default:
                    stockItem.IsCloseToExpirationDate = false;
                    break;
            }

            return true;
        }
    }
}
