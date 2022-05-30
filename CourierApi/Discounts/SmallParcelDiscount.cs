using CourierApi.Extensions;
using CourierApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApi.Discounts
{
    public class SmallParcelDiscount : IDiscount
    {
        private readonly int _numberForValidDiscount = 4;
        public string DiscountOffer {get;}

        public SmallParcelDiscount()
        {
            DiscountOffer = "Small Parcel Mania!";
        }

        public IEnumerable<Discount> CheckDiscount(IList<ParcelOrder> orders)
        {
            var orderedSmall = orders.Where(w => w.SizeType == ParcelSizeEnum.Small && !w.HasBeenDiscounted)
                                .OrderBy(o => o.OverallCost);
            
            if (orderedSmall.Count() < _numberForValidDiscount)
            {
                return null;
            }

            var numberOfDiscounts = Math.Floor((double)(orderedSmall.Count() / _numberForValidDiscount));
            var selectedItems = orderedSmall.ToArray().Take((int)numberOfDiscounts);
            
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