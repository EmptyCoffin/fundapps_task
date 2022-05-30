using CourierApi.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CourierApiUnitTests.Extensions
{
    [TestClass]
    public class IListExtensionsUnitTests
    {
        [TestMethod]
        public void FindIndex_GivenPredicate_ShouldReturnIndexOfFoundItem()
        {
            // arrange
            List<string> testList = new List<string>{"Test","NotATest","Bread","Test"};

            // act
            var result = testList.FindIndex(a => a == "Bread");

            // assert
            Assert.AreEqual(2, result);
        }
        
        [TestMethod]
        public void FindIndex_GivenPredicate_ShouldReturnIndexOfFirstFoundItem()
        {
            // arrange
            List<string> testList = new List<string>{"Test","NotATest","Bread","Test"};

            // act
            var result = testList.FindIndex(a => a == "Test");

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void FindIndex_GivenPredicate_ShouldReturnMinusOneWhenNotFound()
        {
            // arrange
            List<string> testList = new List<string>{"Test","NotATest","Bread","Test"};

            // act
            var result = testList.FindIndex(a => a == "NotBread");

            // assert
            Assert.AreEqual(-1, result);
        }
    }
}