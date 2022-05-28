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
            var input = new ParcelInput[]
            {
                new ParcelInput
                {
                    Dimensions = new int [] {6, 6, 6},
                    Weight = "1Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {36, 36, 36},
                    Weight = "1Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {56, 56, 56},
                    Weight = "1Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {106, 106, 106},
                    Weight = "1Kg"
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

        [TestMethod]
        public void GetParcelPricing_GivenInput_ShouldReturnTotalAmountAndSpeedyShippingAmount()
        {
            // arrange
            var input = new ParcelInput[]
            {
                new ParcelInput
                {
                    Dimensions = new int [] {36, 36, 36},
                    Weight = "1Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {36, 36, 36},
                    Weight = "1Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {106, 106, 106},
                    Weight = "1Kg"
                }
            };

            // act
            var response = _parcelPricingService.GetParcelPricing(input);

            // assert
            Assert.IsNotNull(response);
            Assert.AreEqual("$41.00", response.TotalPrice);
            Assert.AreEqual("$82.00", response.SpeedyShippingPrice);
        }

        [TestMethod]
        public void GetParcelPricing_GivenInputWithExceedingWeights_ShouldReturnHigherTotalAmounts()
        {
            // arrange
            var input = new ParcelInput[]
            {
                new ParcelInput
                {
                    Dimensions = new int [] {36, 36, 36},
                    Weight = "5Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {36, 36, 36},
                    Weight = "1Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {36, 36, 36},
                    Weight = "9Kg"
                },
                new ParcelInput
                {
                    Dimensions = new int [] {106, 106, 106},
                    Weight = "15Kg"
                }
            };

            // act
            var response = _parcelPricingService.GetParcelPricing(input);

            // assert
            Assert.IsNotNull(response);
            Assert.AreEqual("$75.00", response.TotalPrice);
            Assert.AreEqual("$150.00", response.SpeedyShippingPrice);
        }
    }
}