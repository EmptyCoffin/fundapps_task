namespace CourierApi.Models
{
    public class OrderResponse
    {
        public Parcel[] Parcels {get;set;}

        public string TotalPrice {get;set;}
    }
}