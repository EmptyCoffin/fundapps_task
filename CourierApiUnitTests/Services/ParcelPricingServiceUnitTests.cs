using CourierApi.Models;
using CourierApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CourierApiUnitTests.Services
{
    [TestClass]
    public class ParcelPricingServiceUnitTests
    {
        private ParcelPricingService _parcelPricingService;

        [TestInitialize]
        public void Initialise()
        {
            _parcelPricingService = new ParcelPricingService();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _parcelPricingService = null;
        }

        [TestMethod]
        public void GetParcelPricing_GivenDifferentDimensions_ShouldCorrectlyIdentifyParcelsAndReturnTotalCount()
        {
            // arrange
            var input = new ParcelOrder[]
            {
                new ParcelOrder
                {
                    Dimensions = new int [] {6, 6, 6}
                },
                new ParcelOrder
                {
                    Dimensions = new int [] {36, 36, 36}
                },
                new ParcelOrder
                {
                    Dimensions = new int [] {56, 56, 56}
                },
                new ParcelOrder
                {
                    Dimensions = new int [] {106, 106, 106}
                }
            };

            // act
            var response = _parcelPricingService.GetParcelPricing(input);

            // assert
            Assert.IsNotNull(response);
            Assert.AreEqual("$51.00", response.TotalPrice);
            Assert.AreEqual(input.Length, response.Parcels.Length);
            Assert.AreEqual("Small", response.Parcels[0].SizeType);
            Assert.AreEqual("Medium", response.Parcels[1].SizeType);
            Assert.AreEqual("Large", response.Parcels[2].SizeType);
            Assert.AreEqual("XL", response.Parcels[3].SizeType);
        }
    }
}