using CourierApi.Models;
using CourierApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourierApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParcelController : ControllerBase
    {
        private IParcelPricingService _service;

        public ParcelController(IParcelPricingService service)
        {
            _service = service;
        }

        [HttpPost]
        public OrderResponse GetParcelPricing([FromBody] ParcelInput[] orders)
        {
            if(orders == null || orders.Length == 0)
            {
                return new OrderResponse();
            }

            return _service.GetParcelPricing(orders);
        }
    }
}
