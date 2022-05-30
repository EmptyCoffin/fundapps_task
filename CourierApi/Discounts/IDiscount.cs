using CourierApi.Models;
using System.Collections.Generic;

namespace CourierApi.Discounts
{
    public interface IDiscount
    {
        string DiscountOffer {get;}

        IEnumerable<Discount> CheckDiscount(IList<ParcelOrder> orders);
    }
}