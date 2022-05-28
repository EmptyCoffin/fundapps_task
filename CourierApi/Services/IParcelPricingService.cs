using CourierApi.Models;

namespace CourierApi.Services
{
    public interface IParcelPricingService
    {
         OrderResponse GetParcelPricing(ParcelInput[] orders);
    }
}