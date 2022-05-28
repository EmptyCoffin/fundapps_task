using System;
using Microsoft.AspNetCore.Mvc;

namespace CourierApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParcelController : ControllerBase
    {
        [HttpGet]
        public void GetParcelPricing()
        {
            throw new NotImplementedException();
        }
    }
}
