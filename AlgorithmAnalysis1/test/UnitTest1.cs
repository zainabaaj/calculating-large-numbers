using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculateLargeNumber;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Ad()
        {
            LargeNumbers l1 = new LargeNumbers("-3.15");
            LargeNumbers l2 = new LargeNumbers("1.0");
            LargeNumbers sum = LargeNumbers.Add(l1, l2);

            //Assert.AreEqual(NumSign.Negative, sum.Sign);
           // Assert.AreEqual(1, sum.DecimalPlaces);
            CollectionAssert.AreEqual(new int[] { 2,1,5 }, sum.Digits);
        }
        [TestMethod]
        public void Mul()
        {
            LargeNumbers l1 = new LargeNumbers("55555555555555555");
            LargeNumbers l2 = new LargeNumbers("-33333333333333333333333333333");
            LargeNumbers sum = LargeNumbers.Multiply(l1, l2);

            Assert.AreEqual(NumSign.Negative, sum.Sign);
            // Assert.AreEqual(1, sum.DecimalPlaces);
            CollectionAssert.AreEqual(new int[] { 1,3,0,3,5 }, sum.Digits);
        }
        [TestMethod]
        public void Sub()
        {
            LargeNumbers l1 = new LargeNumbers("-3.3");
            LargeNumbers l2 = new LargeNumbers("-39.5");
            LargeNumbers sum = LargeNumbers.Sub(l1, l2);

            Assert.AreEqual(NumSign.Negative, sum.Sign);
            // Assert.AreEqual(1, sum.DecimalPlaces);
            CollectionAssert.AreEqual(new int[] { 3,6,2 }, sum.Digits);
        }
        [TestMethod]
        public void divide()
        {
            LargeNumbers l1 = new LargeNumbers("2.369");
            LargeNumbers l2 = new LargeNumbers("1.36");
            LargeNumbers sum = LargeNumbers.Divide(l1, l2);

            Assert.AreEqual(NumSign.Positive, sum.Sign);
            // Assert.AreEqual(1, sum.DecimalPlaces);
            CollectionAssert.AreEqual(new int[] { 1,7,3,0,4,6,0,1,8,9,9,1,9,6,4,9 }, sum.Digits);
        }
    }
}
