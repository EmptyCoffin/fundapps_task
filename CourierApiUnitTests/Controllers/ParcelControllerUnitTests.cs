using CourierApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CourierApiUnitTests
{
    [TestClass]
    public class ParcelControllerUnitTests
    {
        private ParcelController _parcelController;

        [TestInitialize]
        public void Initialise()
        {
            _parcelController = new ParcelController();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetParcelPricing_ShouldThrowExceptionWhenCalled()
        {
            // act
            _parcelController.GetParcelPricing();
        }
    }
}
