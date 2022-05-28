using CourierApi.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CourierApi.Services
{
    public class ParcelPricingService : IParcelPricingService
    {
        private Parcel[] _availableParcels = new [] {
            new Parcel
            {
                SizeType = "Small",
                Price = 3.0M,
                MaxDimension = 10
            },
            new Parcel
            {
                SizeType = "Medium",
                Price = 8.0M,
                MaxDimension = 50
            },
            new Parcel
            {
                SizeType = "Large",
                Price = 15.0M,
                MaxDimension = 100
            },
            new Parcel
            {
                SizeType = "XL",
                Price = 25.0M,
                MaxDimension = 999
            }
        };

        public OrderResponse GetParcelPricing(ParcelOrder[] orders)
        {
            List<Parcel> parcels = new List<Parcel>();

            foreach(var order in orders)
            {
                parcels.Add(_availableParcels.First(w => order.Dimensions.All(a => a < w.MaxDimension)));
            }            

            var totalPrice = parcels.Sum(s => s.Price);
            return new OrderResponse
            {
                Parcels = parcels.ToArray(),
                TotalPrice = totalPrice.ToString("C", CultureInfo.CreateSpecificCulture("en-US")),
                SpeedyShippingPrice = (totalPrice * 2).ToString("C", CultureInfo.CreateSpecificCulture("en-US"))
            };
        }
    }
}