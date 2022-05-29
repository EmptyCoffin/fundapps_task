using CourierApi.Models;
using CourierApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApiUnitTests.Services
{
    [TestClass]
    public class ParcelPricingServiceUnitTests
    {
        private ParcelPricingService _parcelPricingService;
        private Mock<IDiscountService> _discountServiceMock;
        private Discount[] _returningDiscounts;

        [TestInitialize]
        public void Initialise()
        {
            _discountServiceMock = new Mock<IDiscountService>();
            _discountServiceMock.Setup(s => s.CheckForDiscounts(It.IsAny<IEnumerable<ParcelOrder>>()))
                .Returns(() => _returningDiscounts).Verifiable();

            _parcelPricingService = new ParcelPricingService(_discountServiceMock.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _returningDiscounts = null;
            _discountServiceMock = null;
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
            Assert.IsNull(response.DiscountsApplied);
            Assert.AreEqual(input.Length, response.Parcels.Length);
            Assert.AreEqual(ParcelSizeEnum.Small, response.Parcels[0].SizeType);
            Assert.AreEqual(ParcelSizeEnum.Medium, response.Parcels[1].SizeType);
            Assert.AreEqual(ParcelSizeEnum.Large, response.Parcels[2].SizeType);
            Assert.AreEqual(ParcelSizeEnum.XL, response.Parcels[3].SizeType);
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
            _returningDiscounts = new Discount[] {};

            // act
            var response = _parcelPricingService.GetParcelPricing(input);

            // assert
            Assert.IsNotNull(response);
            Assert.AreEqual("$75.00", response.TotalPrice);
            Assert.AreEqual("$150.00", response.SpeedyShippingPrice);
        }

        [TestMethod]
        public void GetParcelPricing_GivenOrderHasDiscounts_ShouldReturnResponseWithDiscounts()
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
                },
                new ParcelInput
                {
                    Dimensions = new int [] {106, 106, 106},
                    Weight = "1Kg"
                }
            };
            _returningDiscounts = new [] {
                new Discount
                {
                    Savings = 25.0M,
                    DiscountOffer = "Test Discount 1"
                },
                new Discount
                {
                    Savings = 8.0M,
                    DiscountOffer = "Test Discount 2"
                }
            };

            // act
            var response = _parcelPricingService.GetParcelPricing(input);

            // assert
            _discountServiceMock.Verify(v => v.CheckForDiscounts(It.Is<IEnumerable<ParcelOrder>>(o => o.Count() == input.Length)), Times.Once);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DiscountsApplied);
            Assert.AreEqual(_returningDiscounts.Length, response.DiscountsApplied.Length);
            Assert.AreEqual(_returningDiscounts[0].Savings, response.DiscountsApplied[0].Savings);
            Assert.AreEqual(_returningDiscounts[0].DiscountOffer, response.DiscountsApplied[0].DiscountOffer);
            Assert.AreEqual(_returningDiscounts[1].Savings, response.DiscountsApplied[1].Savings);
            Assert.AreEqual(_returningDiscounts[1].DiscountOffer, response.DiscountsApplied[1].DiscountOffer);
            Assert.AreEqual("$33.00", response.TotalPrice);
            Assert.AreEqual("$66.00", response.SpeedyShippingPrice);
        }
    }
}