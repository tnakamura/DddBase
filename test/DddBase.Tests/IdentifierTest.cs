using System;
using Xunit;

namespace DddBase.Tests
{
    public class IdentifierTest
    {
        public class TestId : Identifier<int>
        {
            public TestId(int value)
                : base(value)
            {
            }
        }

        public class ExtendedTestId : TestId
        {
            public ExtendedTestId(int value)
                : base(value)
            {
            }
        }

        [Fact]
        public void EqualsTest()
        {
            var testId1 = new TestId(10);
            Assert.False(testId1.Equals(null));
            Assert.True(testId1.Equals(testId1));

            var testId2 = new TestId(10);
            Assert.True(testId1.Equals(testId2));

            var testId3 = new TestId(11);
            Assert.False(testId1.Equals(testId3));

            var extendedTestId1 = new ExtendedTestId(10);
            Assert.False(testId1.Equals(extendedTestId1));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var testId1 = new TestId(10);
            var testId2 = new TestId(10);
            Assert.Equal(testId1.GetHashCode(), testId2.GetHashCode());

            var testId3 = new TestId(11);
            Assert.NotEqual(testId1.GetHashCode(), testId3.GetHashCode());
        }

        [Fact]
        public void EqualsOperatorTest()
        {
            var testId1 = new TestId(10);
            Assert.False(testId1 == null);

            var testId2 = new TestId(10);
            Assert.True(testId1 == testId2);

            var testId3 = new TestId(11);
            Assert.False(testId1 == testId3);

            var extendedTestId1 = new ExtendedTestId(10);
            Assert.False(testId1 == extendedTestId1);
        }

        [Fact]
        public void NotEqualsOperatorTest()
        {
            var testId1 = new TestId(10);
            Assert.True(testId1 != null);

            var testId2 = new TestId(10);
            Assert.False(testId1 != testId2);

            var testId3 = new TestId(11);
            Assert.True(testId1 != testId3);

            var extendedTestId1 = new ExtendedTestId(10);
            Assert.True(testId1 != extendedTestId1);
        }

        [Fact]
        public void CompareTest()
        {
            var testId1 = new TestId(10);
            Assert.Equal(1, testId1.CompareTo(null));

            var testId2 = new TestId(10);
            Assert.Equal(0, testId1.CompareTo(testId2));

            var testId3 = new TestId(11);
            Assert.Equal(-1, testId1.CompareTo(testId3));

            var testId4 = new TestId(9);
            Assert.Equal(1, testId1.CompareTo(testId4));
        }
    }
}
