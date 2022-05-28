namespace CourierApi.Models
{
    public class ParcelTemplate
    {
        public string SizeType {get;set;}

        public int MaxDimension {get;set;}

        public decimal Price {get;set;}

        public int WeightLimit {get;set;}

        public decimal CostPerWeightExceeded {get;set;}
    }
}