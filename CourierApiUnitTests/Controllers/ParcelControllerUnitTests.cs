using CourierApi.Controllers;
using CourierApi.Models;
using CourierApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CourierApiUnitTests
{
    [TestClass]
    public class ParcelControllerUnitTests
    {
        private ParcelController _parcelController;
        private Mock<IParcelPricingService> _parcelPricingServiceMock;
        private OrderResponse _orderResponse;

        [TestInitialize]
        public void Initialise()
        {
            _parcelPricingServiceMock = new Mock<IParcelPricingService>();
            _parcelPricingServiceMock.Setup(s => s.GetParcelPricing(It.IsAny<ParcelInput[]>()))
                .Returns(() => _orderResponse).Verifiable();

            _parcelController = new ParcelController(_parcelPricingServiceMock.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _orderResponse = null;
            _parcelPricingServiceMock = null;
            _parcelController = null;
        }

        [DataRow(null)]
        [DataRow(0)]
        [DataTestMethod]
        public void GetParcelPricing_GivenInvalidInput_ShouldReturnEmptyResponse(int? arrayLength)
        {
            // arrange
            var input = arrayLength.HasValue ? new ParcelInput[arrayLength.Value] : null;

            // act
            var response = _parcelController.GetParcelPricing(input);

            // assert
            Assert.IsNotNull(response);
            Assert.IsNull(response.Parcels);
            Assert.IsNull(response.TotalPrice);
        }

        [TestMethod]
        public void GetParcelPricing_GivenInput_ShouldReturnValueFromService()
        {
            // arrange
            var inputDimensions = new [] {
                new ParcelInput { 
                    Dimensions = new [] { 1, 1, 1 }
                 }
            };
            _orderResponse = new OrderResponse
            {
                Parcels = new [] {
                    new ParcelOrder {
                        SizeType = "Small",
                        OverallCost = 2.0M
                    }
                },
                TotalPrice = "$2.00"
            };

            // act
            var response = _parcelController.GetParcelPricing(inputDimensions);

            // assert
            _parcelPricingServiceMock.Verify(v => v.GetParcelPricing(It.Is<ParcelInput[]>(p => p.Length == inputDimensions.Length)), Times.Once);
            Assert.IsNotNull(response);
            Assert.AreEqual(_orderResponse.TotalPrice, response.TotalPrice);
            Assert.AreEqual(_orderResponse.Parcels.Length, response.Parcels.Length);
            Assert.AreEqual(_orderResponse.Parcels[0].SizeType, response.Parcels[0].SizeType);
        }
    }
}
