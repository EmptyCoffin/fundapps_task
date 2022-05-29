using CourierApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApi.Discounts
{
    public class MixedParcelDiscount : IDiscount
    {
        private readonly int _numberForValidDiscount = 5;
        public string DiscountOffer {get;}

        public MixedParcelDiscount()
        {
            DiscountOffer = "Mixed Parcel Mania!";
        }

        public IEnumerable<Discount> CheckDiscount(IEnumerable<ParcelOrder> orders)
        {
            var ordered = orders.OrderBy(o => o.OverallCost);
            
            if (ordered.Count() < _numberForValidDiscount)
            {
                return null;
            }

            var numberOfDiscounts = Math.Floor((double)(ordered.Count() / _numberForValidDiscount));
            return ordered.ToArray()
                .Take((int)numberOfDiscounts).Select(s => 
                    new Discount
                    { 
                        Savings = s.OverallCost, 
                        DiscountOffer = DiscountOffer 
                    });
        }
    }
}