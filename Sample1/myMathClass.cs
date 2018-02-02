using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Sample1
{
    public class myMathClass
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public double Area(double length, double width)
        {
            return length * width;
        }

        public bool IsOdd(int number)
        {
            return number % 2 == 1;
        }

    }

    // Data Generator for Theory with ClassData
    public class AreaTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>
            {
                new object[] {5, 7, 35},
                new object[] {6, 8, 48},
                new object[] {8, 9, 72}
            };
        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class AnotherAreaTestDataGenerator
    {
        public static IEnumerable<object[]> GetDataForAreaTest()
        {
            yield return new object[] { 11, 11, 121 };
            yield return new object[] { 12, 12, 144 };
        }
    }
}
