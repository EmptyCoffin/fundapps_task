using CourierApi.Extensions;
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

        public IEnumerable<Discount> CheckDiscount(IList<ParcelOrder> orders)
        {
            var ordered = orders.Where(w => !w.HasBeenDiscounted).OrderBy(o => o.OverallCost);

            if (ordered.Count() < _numberForValidDiscount)
            {
                return null;
            }

            var numberOfDiscounts = Math.Floor((double)(ordered.Count() / _numberForValidDiscount));
            var selectedItems = ordered.ToArray().Take((int)numberOfDiscounts);
            
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