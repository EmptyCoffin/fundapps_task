namespace CourierApi.Models
{
    public class OrderResponse
    {
        public ParcelOrder[] Parcels {get;set;}

        public Discount[] DiscountsApplied {get;set;}

        public string TotalPrice {get;set;}

        public string SpeedyShippingPrice {get;set;}
    }
}