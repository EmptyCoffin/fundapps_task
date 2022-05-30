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
            var orders = new List<ParcelOrder> {
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
                    SizeType = ParcelSizeEnum.XL
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Large
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Medium
                },
                new ParcelOrder
                {
                    SizeType = ParcelSizeEnum.Small,
                    HasBeenDiscounted = true
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
            var orders = new List<ParcelOrder> {
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
            Assert.IsFalse(orders[0].HasBeenDiscounted);
            Assert.IsTrue(orders[1].HasBeenDiscounted);
            Assert.IsFalse(orders[2].HasBeenDiscounted);
            Assert.IsFalse(orders[3].HasBeenDiscounted);
            Assert.IsFalse(orders[4].HasBeenDiscounted);
            Assert.AreEqual(8.0M, result[0].Savings);
        }

        [TestMethod]
        public void CheckDiscount_GivenMultipleCorrectMixedParcels_ShouldReturnWithCheapestSetAsDiscount()
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
            
            Assert.IsFalse(orders[0].HasBeenDiscounted);
            Assert.IsFalse(orders[1].HasBeenDiscounted);
            Assert.IsFalse(orders[2].HasBeenDiscounted);
            Assert.IsFalse(orders[3].HasBeenDiscounted);
            Assert.IsFalse(orders[4].HasBeenDiscounted);
            Assert.IsFalse(orders[5].HasBeenDiscounted);
            Assert.IsTrue(orders[6].HasBeenDiscounted);
            Assert.IsFalse(orders[7].HasBeenDiscounted);
            Assert.IsTrue(orders[8].HasBeenDiscounted);
            Assert.IsFalse(orders[9].HasBeenDiscounted);
            Assert.IsFalse(orders[10].HasBeenDiscounted);
        }      
    }
}