using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ClientService;
namespace XunitTestClass
{
    public class testClient
    {
        
        [Fact]
        public void goodClientLastNameTest()
        {
            Client bob = new Client("Bob Smith");
            Assert.Equal("Smith", bob.LastName);
        }

        [Fact]
        public void goodGetClientFullName()
        {
            Client dave = new Client("David", "Finch");
            Assert.Equal("David Finch", dave.FullName);
        }

        [Fact]
        public void badClientFullNameTest()
        {
            Action a = () => new Client("Steve W I L D Madden");
            Assert.Throws<ArgumentException>(a);
        }
    }
}
