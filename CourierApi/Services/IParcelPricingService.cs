using CourierApi.Models;

namespace CourierApi.Services
{
    public interface IParcelPricingService
    {
         OrderResponse GetParcelPricing(ParcelOrder[] orders);
    }
}