namespace CourierApi.Models
{
    public class ParcelOrder
    {
        public ParcelSizeEnum SizeType {get;set;}

        public bool HasBeenDiscounted {get;set;}

        public decimal OverallCost {get;set;}
    }
}