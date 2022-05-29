using CourierApi.Discounts;
using CourierApi.Models;
using System.Collections.Generic;

namespace CourierApi.Services
{
    public interface IDiscountService
    {
        IEnumerable<Discount> CheckForDiscounts(IEnumerable<ParcelOrder> orders);
    }
}