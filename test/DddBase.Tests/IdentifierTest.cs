using System;
using Xunit;

namespace DddBase.Tests
{
    public class IdentifierTest
    {
        public class TestId : Identifier<Guid>
        {
            public TestId()
                : base(Guid.NewGuid())
            {
            }

            public TestId(Guid value)
                : base(value)
            {
            }
        }

        [Fact]
        public void EqualsTest()
        {
            var id1 = new TestId(new Guid("E695C938-8B69-44F6-87C6-F7DDA6916AF2"));
            Assert.True(id1.Equals(id1));

            var id2 = new TestId(new Guid("E695C938-8B69-44F6-87C6-F7DDA6916AF2"));
            Assert.True(id1.Equals(id2));

            var id3 = new TestId(new Guid("6A34B80D-FD96-4C81-86A1-3E71AEF4C1F1"));
            Assert.False(id1.Equals(id3));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var id1 = new TestId(new Guid("F25FC16D-CC9D-4CDC-83F7-A018FE12FB49"));
            var id2 = new TestId(new Guid("F25FC16D-CC9D-4CDC-83F7-A018FE12FB49"));
            Assert.Equal(id1.GetHashCode(), id2.GetHashCode());

            var id3 = new TestId(new Guid("96ED5693-0E3D-496E-9DB4-767A3F896BAB"));
            Assert.NotEqual(id1.GetHashCode(), id3.GetHashCode());
        }
    }
}
