using CourierApi.Discounts;
using CourierApi.Models;
using System.Collections.Generic;

namespace CourierApi.Services
{
    public class DiscountsService : IDiscountService
    {
        private IEnumerable<IDiscount> _availableDiscounts;

        public DiscountsService(IEnumerable<IDiscount> availableDiscounts)
        {
            _availableDiscounts = availableDiscounts;
        }

        public IEnumerable<Discount> CheckForDiscounts(IEnumerable<ParcelOrder> orders)
        {
            List<Discount> discounts = new List<Discount>();

            foreach(var discount in _availableDiscounts)
            {
                var discountsFound = discount.CheckDiscount(orders);
                if (discountsFound != null)
                {
                    discounts.AddRange(discountsFound);
                }
            }

            return discounts.ToArray();
        }
    }
}