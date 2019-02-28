using System;
using Xunit;

namespace DddBase.Tests
{
    public class EntityTest
    {
        public class TestEntity : Entity<Guid>
        {
            public TestEntity(Guid id)
                : base(id)
            {
            }
        }

        [Fact]
        public void EqualsTest()
        {
            var testEntity1 = new TestEntity(new Guid("947F80CD-9C0B-45B1-B82E-6C3DAD74DE80"));
            Assert.False(testEntity1.Equals(null));
            Assert.True(testEntity1.Equals(testEntity1));

            var testEntity2 = new TestEntity(new Guid("947F80CD-9C0B-45B1-B82E-6C3DAD74DE80"));
            Assert.True(testEntity1.Equals(testEntity2));

            var testEntity3 = new TestEntity(new Guid("01DBB420-31D0-4A2D-B471-7EED7F3660DE"));
            Assert.False(testEntity1.Equals(testEntity3));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var testEntity1 = new TestEntity(new Guid("09D6A5A9-1D67-4D4B-B287-CEC42D74926A"));
            var testEntity2 = new TestEntity(new Guid("09D6A5A9-1D67-4D4B-B287-CEC42D74926A"));
            Assert.Equal(testEntity1.GetHashCode(), testEntity2.GetHashCode());

            var testEntity3 = new TestEntity(new Guid("A1336BAB-33A7-4FD5-8339-5F518BE2716F"));
            Assert.NotEqual(testEntity1.GetHashCode(), testEntity3.GetHashCode());
        }
    }
}
