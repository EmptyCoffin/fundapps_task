using CourierApi.Discounts;
using CourierApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CourierApiUnitTests.Discounts
{
    [TestClass]
    public class MediumParcelDiscountUnitTests
    {
        private MediumParcelDiscount _mediumParcelDiscount;

        [TestInitialize]
        public void Initialise()
        {
            _mediumParcelDiscount = new MediumParcelDiscount();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mediumParcelDiscount = null;
        }
        
        [TestMethod]
        public void DiscountOffer_GivenClassInitialised_ShouldReturnCorrectValue()
        {
            // act
            var result = _mediumParcelDiscount.DiscountOffer;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Medium Parcel Mania!", result);
        }

        [TestMethod]
        public void CheckDiscount_GivenNotEnoughMediumParcels_ShouldReturnNull()
        {
            // arrange
            var orders = new List<ParcelOrder> {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium
                }
            };

            // act
            var result = _mediumParcelDiscount.CheckDiscount(orders);

            // assert
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void CheckDiscount_GivenParcelsHaveBeenDiscounted_ShouldNotApplyDiscount()
        {
            // arrange
            var orders = new List<ParcelOrder> {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    HasBeenDiscounted = true
                }
            };

            // act
            var result = _mediumParcelDiscount.CheckDiscount(orders);

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CheckDiscount_GivenCorrectMediumParcels_ShouldReturnWithCheapestSetAsDiscount()
        {
            // arrange
            var orders = new List<ParcelOrder> {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
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
                }
            };

            // act
            var result = _mediumParcelDiscount.CheckDiscount(orders).ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Medium Parcel Mania!", result[0].DiscountOffer);
            Assert.AreEqual(8.0M, result[0].Savings);
            Assert.IsTrue(orders[1].HasBeenDiscounted);
            Assert.IsFalse(orders[0].HasBeenDiscounted);
            Assert.IsFalse(orders[2].HasBeenDiscounted);
            Assert.IsFalse(orders[3].HasBeenDiscounted);
        }

        [TestMethod]
        public void CheckDiscount_GivenMultipleCorrectMediumParcels_ShouldReturnWithCheapestSetAsDiscount()
        {
            // arrange
            var orders = new List<ParcelOrder> {
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 12.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 8.0M
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 30.0M
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
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 6.0M
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
                    SizeType = ParcelSizeEnum.Medium,
                    OverallCost = 30.0M
                },
            };

            // act
            var result = _mediumParcelDiscount.CheckDiscount(orders).ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Medium Parcel Mania!", result[0].DiscountOffer);
            Assert.AreEqual(6.0M, result[0].Savings);
            Assert.AreEqual("Medium Parcel Mania!", result[1].DiscountOffer);
            Assert.AreEqual(8.0M, result[1].Savings);
            Assert.IsFalse(orders[0].HasBeenDiscounted);
            Assert.IsTrue(orders[1].HasBeenDiscounted);
            Assert.IsFalse(orders[2].HasBeenDiscounted);
            Assert.IsFalse(orders[3].HasBeenDiscounted);
            Assert.IsFalse(orders[4].HasBeenDiscounted);
            Assert.IsTrue(orders[5].HasBeenDiscounted);
            Assert.IsFalse(orders[6].HasBeenDiscounted);
            Assert.IsFalse(orders[7].HasBeenDiscounted);
            Assert.IsFalse(orders[8].HasBeenDiscounted);
        }
    }
}