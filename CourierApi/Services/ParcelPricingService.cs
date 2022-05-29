using CourierApi.Models;
using CourierApi.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CourierApi.Services
{
    public class ParcelPricingService : IParcelPricingService
    {
        private IDiscountService _discountService;
        private ParcelTemplate[] _availableParcels = new [] {
            new ParcelTemplate
            {
                SizeType = ParcelSizeEnum.Small,
                Price = 3.0M,
                MaxDimension = 10,
                WeightLimit = 1,
                CostPerWeightExceeded = 2.0M
            },
            new ParcelTemplate
            {
                SizeType = ParcelSizeEnum.Medium,
                Price = 8.0M,
                MaxDimension = 50,
                WeightLimit = 3,
                CostPerWeightExceeded = 2.0M
            },
            new ParcelTemplate
            {
                SizeType = ParcelSizeEnum.Large,
                Price = 15.0M,
                MaxDimension = 100,
                WeightLimit = 6,
                CostPerWeightExceeded = 2.0M
            },
            new ParcelTemplate
            {
                SizeType = ParcelSizeEnum.XL,
                Price = 25.0M,
                MaxDimension = 999,
                WeightLimit = 10,
                CostPerWeightExceeded = 2.0M
            }
        };

        public ParcelPricingService(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public OrderResponse GetParcelPricing(ParcelInput[] orders)
        {
            List<ParcelOrder> parcels = new List<ParcelOrder>();

            foreach(var order in orders)
            {
                parcels.Add(GetParcelOrder(order));
            }            

            var discounts = _discountService.CheckForDiscounts(parcels);

            var totalPrice = parcels.Sum(s => s.OverallCost) - (discounts?.Sum(s => s.Savings) ?? 0);
            return new OrderResponse
            {
                Parcels = parcels.ToArray(),
                DiscountsApplied = discounts?.ToArray(),
                TotalPrice = totalPrice.ToString("C", CultureInfo.CreateSpecificCulture("en-US")),
                SpeedyShippingPrice = (totalPrice * 2).ToString("C", CultureInfo.CreateSpecificCulture("en-US"))
            };
        }

        private ParcelOrder GetParcelOrder(ParcelInput order)
        {
            var selectedParcel = _availableParcels.First(w => order.Dimensions.All(a => a < w.MaxDimension));
            var returningParcel = new ParcelOrder {
                SizeType = selectedParcel.SizeType,
                OverallCost = selectedParcel.Price
            };

            var parcelWeight = Convert.ToInt32(order.Weight.Substring(0, order.Weight.Length - 2));
            if (parcelWeight > selectedParcel.WeightLimit)
            {
                returningParcel.OverallCost += (parcelWeight - selectedParcel.WeightLimit) * selectedParcel.CostPerWeightExceeded;
            }

            return returningParcel;
        }
    }
}