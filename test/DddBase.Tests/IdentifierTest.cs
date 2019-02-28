using System;
using Xunit;

namespace DddBase.Tests
{
    public class IdentifierTest
    {
        public class TestId : Identifier<Guid>
        {
            public TestId(Guid value)
                : base(value)
            {
            }
        }

        public class ExtendedTestId : TestId
        {
            public ExtendedTestId(Guid value)
                : base(value)
            {
            }
        }

        [Fact]
        public void EqualsTest()
        {
            var testId1 = new TestId(new Guid("E695C938-8B69-44F6-87C6-F7DDA6916AF2"));
            Assert.False(testId1.Equals(null));
            Assert.True(testId1.Equals(testId1));

            var testId2 = new TestId(new Guid("E695C938-8B69-44F6-87C6-F7DDA6916AF2"));
            Assert.True(testId1.Equals(testId2));

            var testId3 = new TestId(new Guid("6A34B80D-FD96-4C81-86A1-3E71AEF4C1F1"));
            Assert.False(testId1.Equals(testId3));

            var extendedTestId1 = new ExtendedTestId(new Guid("E695C938-8B69-44F6-87C6-F7DDA6916AF2"));
            Assert.False(testId1.Equals(extendedTestId1));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var testId1 = new TestId(new Guid("F25FC16D-CC9D-4CDC-83F7-A018FE12FB49"));
            var testId2 = new TestId(new Guid("F25FC16D-CC9D-4CDC-83F7-A018FE12FB49"));
            Assert.Equal(testId1.GetHashCode(), testId2.GetHashCode());

            var testId3 = new TestId(new Guid("96ED5693-0E3D-496E-9DB4-767A3F896BAB"));
            Assert.NotEqual(testId1.GetHashCode(), testId3.GetHashCode());
        }
    }
}
