using CourierApi.Discounts;
using CourierApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CourierApiUnitTests.Discounts
{
    [TestClass]
    public class MixedParcelDiscountUnitTests
    {
        private MixedParcelDiscount _mixedParcelDiscount;

        [TestInitialize]
        public void Initialise()
        {
            _mixedParcelDiscount = new MixedParcelDiscount();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mixedParcelDiscount = null;
        }
        
        [TestMethod]
        public void DiscountOffer_GivenClassInitialised_ShouldReturnCorrectValue()
        {
            // act
            var result = _mixedParcelDiscount.DiscountOffer;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Mixed Parcel Mania!", result);
        }

        [TestMethod]
        public void CheckDiscount_GivenNotEnoughMixedParcels_ShouldReturnNull()
        {
            // arrange
            var orders = new ParcelOrder[] {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium
                }
            };

            // act
            var result = _mixedParcelDiscount.CheckDiscount(orders);

            // assert
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void CheckDiscount_GivenCorrectMixedParcels_ShouldReturnWithCheapestSetAsDiscount()
        {
            // arrange
            var orders = new ParcelOrder[] {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.XL,
                    OverallCost = 8.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Large,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 30.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 12.0M
                }
            };

            // act
            var result = _mixedParcelDiscount.CheckDiscount(orders).ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Mixed Parcel Mania!", result[0].DiscountOffer);
            Assert.AreEqual(8.0M, result[0].Savings);
        }

        [TestMethod]
        public void CheckDiscount_GivenMultipleCorrectMixedParcels_ShouldReturnWithCheapestSetAsDiscount()
        {
            // arrange
            var orders = new ParcelOrder[] {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 8.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 30.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Large,
                    OverallCost = 4.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 6.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 30.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 12.0M
                }
            };

            // act
            var result = _mixedParcelDiscount.CheckDiscount(orders).ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Mixed Parcel Mania!", result[0].DiscountOffer);
            Assert.AreEqual(4.0M, result[0].Savings);
            Assert.AreEqual("Mixed Parcel Mania!", result[1].DiscountOffer);
            Assert.AreEqual(6.0M, result[1].Savings);
        }      
    }
}