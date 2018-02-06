using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Sample1;
namespace XunitTestClass
{
    public class testMathClass
    {
        myMathClass mathClass = new myMathClass();

        // Facts are tests which are always true. They test invariant conditions.

        [Fact]
        public void goodAreaTest()
        {
            var area = mathClass.Area(5, 6);

            Assert.Equal(30, area);
        }

        // A test that fails
        //[Fact]
        //public void badAreaTest()
        //{
        //    var area = mathClass.Area(6, 6);

        //    Assert.Equal(35, area);
        //}

        [Fact]

        public void goodAdditionTest()
        {
            var sum = mathClass.Add(5, 12);
            Assert.Equal(17, sum);
        }

        // Theories are tests which are only true for a particular set of data.
        // Test with Theory InlineData, only simple data type can be passed to InlineData

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(8)]
        public void isOddTestWithWrongData(int number)
        {
            Assert.True(mathClass.IsOdd(number));
        }
        [Theory]
        [InlineData(3, 5, 15)]
        [InlineData(6, 8, 48)]
        [InlineData(12, 7, 84)]
        public void areaTestWithTheory(double length, double width, double expectedArea)
        {
            double area = mathClass.Area(length, width);
            Assert.Equal(expectedArea, area);
        }

        // Test with Theory ClassData, more flexibility, test other data types
        [Theory]
        [ClassData(typeof(AreaTestDataGenerator))]
        public void areaTestWithClassData(double length, double width, double expectedArea)
        {
            double area = mathClass.Area(length, width);
            Assert.Equal(expectedArea, area);
        }

        // Test with Theory MemberData, like ClassData but does not require class
        public static IEnumerable<object[]> localAreaData()
        {
            yield return new object[] { 9, 9, 81 };
            yield return new object[] { 1, 9, 9 };
        }
        // local method
        [Theory]
        [MemberData(nameof(localAreaData))]
        public void localAreaTestWithMemberData(double length, double width, double expectedArea)
        {
            double area = mathClass.Area(length, width);
            Assert.Equal(expectedArea, area);
        }

        // non-local method
        [Theory]
        [MemberData(nameof(AnotherAreaTestDataGenerator.GetDataForAreaTest), MemberType = typeof(AnotherAreaTestDataGenerator))]
        public void nonlocalAreaTestwithMemberData(double length, double width, double expectedArea)
        {
            double area = mathClass.Area(length, width);
            Assert.Equal(expectedArea, area);
        }
    }
}
