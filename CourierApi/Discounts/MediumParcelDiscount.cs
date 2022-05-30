using CourierApi.Extensions;
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

        public IEnumerable<Discount> CheckDiscount(IList<ParcelOrder> orders)
        {
            var orderMedium = orders.Where(w => w.SizeType == ParcelSizeEnum.Medium && !w.HasBeenDiscounted)
                                .OrderBy(o => o.OverallCost);
            
            if (orderMedium.Count() < _numberForValidDiscount)
            {
                return null;
            }

            var numberOfDiscounts = Math.Floor((double)(orderMedium.Count() / _numberForValidDiscount));
            var selectedItems = orderMedium.ToArray().Take((int)numberOfDiscounts);
            
            foreach(var selectedItem in selectedItems)
            {
                var index = orders.FindIndex(f => f.SizeType == selectedItem.SizeType 
                                && f.OverallCost == selectedItem.OverallCost && !f.HasBeenDiscounted);
                orders[index].HasBeenDiscounted = true;
            }

            return selectedItems.Select(s => 
                    new Discount
                    { 
                        Savings = s.OverallCost, 
                        DiscountOffer = DiscountOffer 
                    });
        }
    }
}