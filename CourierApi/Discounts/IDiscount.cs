using CourierApi.Models;
using System.Collections.Generic;

namespace CourierApi.Discounts
{
    public interface IDiscount
    {
        string DiscountOffer {get;}

        IEnumerable<Discount> CheckDiscount(IEnumerable<ParcelOrder> orders);
    }
}