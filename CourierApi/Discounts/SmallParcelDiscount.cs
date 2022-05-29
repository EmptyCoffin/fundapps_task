using CourierApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApi.Discounts
{
    public class SmallParcelDiscount : IDiscount
    {
        public string DiscountOffer {get;}

        public SmallParcelDiscount()
        {
            DiscountOffer = "Small Parcel Mania!";
        }

        public IEnumerable<Discount> CheckDiscount(IEnumerable<ParcelOrder> orders)
        {
            var orderedSmall = orders.Where(w => w.SizeType == ParcelSizeEnum.Small).OrderBy(o => o.OverallCost);
            
            if (orderedSmall.Count() < 4)
            {
                return null;
            }

            var numberOfDiscounts = Math.Floor((double)(orderedSmall.Count() / 4));
            return orderedSmall.ToArray()
                .Take((int)numberOfDiscounts).Select(s => 
                    new Discount
                    { 
                        Savings = s.OverallCost, 
                        DiscountOffer = DiscountOffer 
                    });
        }
    }
}