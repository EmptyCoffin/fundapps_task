using CourierApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApi.Discounts
{
    public class MediumParcelDiscount : IDiscount
    {
        private readonly int _numberForValidDiscount = 3;
        public string DiscountOffer {get;}
        
        public MediumParcelDiscount()
        {
            DiscountOffer = "Medium Parcel Mania!";   
        }

        public IEnumerable<Discount> CheckDiscount(IEnumerable<ParcelOrder> orders)
        {
            var orderMedium = orders.Where(w => w.SizeType == ParcelSizeEnum.Medium).OrderBy(o => o.OverallCost);
            
            if (orderMedium.Count() < _numberForValidDiscount)
            {
                return null;
            }

            var numberOfDiscounts = Math.Floor((double)(orderMedium.Count() / _numberForValidDiscount));
            return orderMedium.ToArray()
                .Take((int)numberOfDiscounts).Select(s => 
                    new Discount
                    { 
                        Savings = s.OverallCost, 
                        DiscountOffer = DiscountOffer 
                    });
        }
    }
}