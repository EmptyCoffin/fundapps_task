using CourierApi.Discounts;
using CourierApi.Models;
using CourierApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace CourierApiUnitTests.Services
{
    [TestClass]
    public class DiscountServiceUnitTests
    {
        private Mock<IDiscount> _discount1;
        private Mock<IDiscount> _discount2;
        private Discount[] _returningDiscounts1;
        private Discount[] _returningDiscounts2;
        private DiscountsService _service;

        [TestInitialize]
        public void Initialise()
        {
            _discount1 = new Mock<IDiscount>();
            _discount1.Setup(s => s.CheckDiscount(It.IsAny<IList<ParcelOrder>>())).Returns(() => _returningDiscounts1).Verifiable();
            _discount2 = new Mock<IDiscount>();
            _discount2.Setup(s => s.CheckDiscount(It.IsAny<IList<ParcelOrder>>())).Returns(() => _returningDiscounts2).Verifiable();

            _service = new DiscountsService(new [] { _discount1.Object, _discount2.Object });
        }

        [TestCleanup]
        public void CleanUp()
        {
            _discount1 = null;
            _discount2 = null;
            _service = null;
        }

        [TestMethod]
        public void CheckForDiscounts_GivenTwoDiscounts_ShouldCallEachCheckWithOrders()
        {
            // arrange
            var input = new [] {
                new ParcelOrder(), new ParcelOrder(), new ParcelOrder()
            };

            // act
            var result = _service.CheckForDiscounts(input);

            // assert
            _discount1.Verify(v => v.CheckDiscount(It.Is<IList<ParcelOrder>>(o => o.Count() == input.Length)), Times.Once);
            _discount2.Verify(v => v.CheckDiscount(It.Is<IList<ParcelOrder>>(o => o.Count() == input.Length)), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckForDiscounts_GivenTwoReturningDiscounts_ShouldReturnDiscounts()
        {
            // arrange
            var input = new [] {
                new ParcelOrder(), new ParcelOrder(), new ParcelOrder()
            };
            _returningDiscounts1 = new [] {
                new Discount { Savings = 1.0M, DiscountOffer = "Test Offer 2" }
            };
            _returningDiscounts2 = new [] {
                new Discount { Savings = 10.0M, DiscountOffer = "Test Offer" },
                new Discount { Savings = 10.0M, DiscountOffer = "Test Offer" }
            };

            // act
            var result = _service.CheckForDiscounts(input).ToArray();

            // assert
            _discount1.Verify(v => v.CheckDiscount(It.Is<IList<ParcelOrder>>(o => o.Count() == input.Length)), Times.Once);
            _discount2.Verify(v => v.CheckDiscount(It.Is<IList<ParcelOrder>>(o => o.Count() == input.Length)), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(_returningDiscounts1.Length + _returningDiscounts2.Length, result.Count());
            Assert.AreEqual(_returningDiscounts1[0].Savings, result[0].Savings);
            Assert.AreEqual(_returningDiscounts1[0].DiscountOffer, result[0].DiscountOffer);
            Assert.AreEqual(_returningDiscounts2[0].Savings, result[1].Savings);
            Assert.AreEqual(_returningDiscounts2[0].DiscountOffer, result[1].DiscountOffer);
            Assert.AreEqual(_returningDiscounts2[1].Savings, result[2].Savings);
            Assert.AreEqual(_returningDiscounts2[1].DiscountOffer, result[2].DiscountOffer);
        }        
    }
}