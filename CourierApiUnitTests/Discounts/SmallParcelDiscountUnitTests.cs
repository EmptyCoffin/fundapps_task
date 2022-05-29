using CourierApi.Discounts;
using CourierApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CourierApiUnitTests.Discounts
{
    [TestClass]
    public class SmallParcelDiscountUnitTests
    {
        private SmallParcelDiscount _smallParcelDiscount;

        [TestInitialize]
        public void Initialise()
        {
            _smallParcelDiscount = new SmallParcelDiscount();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _smallParcelDiscount = null;
        }
        
        [TestMethod]
        public void DiscountOffer_GivenClassInitialised_ShouldReturnCorrectValue()
        {
            // act
            var result = _smallParcelDiscount.DiscountOffer;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Small Parcel Mania!", result);
        }

        [TestMethod]
        public void CheckDiscount_GivenNotEnoughSmallParcels_ShouldReturnNull()
        {
            // arrange
            var orders = new ParcelOrder[] {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium
                }
            };

            // act
            var result = _smallParcelDiscount.CheckDiscount(orders);

            // assert
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void CheckDiscount_GivenCorrectSmallParcels_ShouldReturnWithCheapestSetAsDiscount()
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
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 8.0M
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
            var result = _smallParcelDiscount.CheckDiscount(orders).ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Small Parcel Mania!", result[0].DiscountOffer);
            Assert.AreEqual(8.0M, result[0].Savings);
        }

        [TestMethod]
        public void CheckDiscount_GivenMultipleCorrectSmallParcels_ShouldReturnWithCheapestSetAsDiscount()
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
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 8.0M
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
                },
                                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
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
            var result = _smallParcelDiscount.CheckDiscount(orders).ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Small Parcel Mania!", result[0].DiscountOffer);
            Assert.AreEqual(6.0M, result[0].Savings);
            Assert.AreEqual("Small Parcel Mania!", result[1].DiscountOffer);
            Assert.AreEqual(8.0M, result[1].Savings);
        }
    }
}