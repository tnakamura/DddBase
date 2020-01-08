using Xunit;

namespace DddBase.Tests
{
    public class ValueObjectHelperTest
    {
        struct FullName
        {
            public FullName(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public string FirstName { get; }

            public readonly string LastName;

            public override bool Equals(object obj) =>
                ValueObjectHelper.Equals<FullName>(this, obj);

            public override int GetHashCode() =>
                ValueObjectHelper.GetHashCode<FullName>(this);

            public static bool operator ==(FullName obj1, FullName obj2) =>
                ValueObjectHelper.EqualsOperator(obj1, obj2);

            public static bool operator !=(FullName obj1, FullName obj2) =>
                ValueObjectHelper.NotEqualsOperator(obj1, obj2);
        }

        [Fact]
        public void EqualsTest()
        {
            var fullName1 = new FullName("Shinji", "Kagawa");
            Assert.False(fullName1.Equals(null));
            Assert.True(fullName1.Equals(fullName1));

            var fullName2 = new FullName("Shinji", "Kagawa");
            Assert.True(fullName1.Equals(fullName2));

            var fullName3 = new FullName("Shinji", "Okazaki");
            Assert.False(fullName1.Equals(fullName3));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var fullName1 = new FullName("Shinji", "Kagawa");
            var fullName2 = new FullName("Shinji", "Kagawa");
            Assert.Equal(fullName1.GetHashCode(), fullName2.GetHashCode());

            var fullName3 = new FullName("Shinji", "Okazaki");
            Assert.NotEqual(fullName1.GetHashCode(), fullName3.GetHashCode());
        }

        [Fact]
        public void EqualsOperatorTest()
        {
            var fullName1 = new FullName("Shinji", "Kagawa");
            Assert.False(fullName1 == null);

            var fullName2 = new FullName("Shinji", "Kagawa");
            Assert.True(fullName1 == fullName2);

            var fullName3 = new FullName("Shinji", "Okazaki");
            Assert.False(fullName1 == fullName3);
        }

        [Fact]
        public void NotEqualsOperatorTest()
        {
            var fullName1 = new FullName("Shinji", "Kagawa");
            Assert.True(fullName1 != null);

            var fullName2 = new FullName("Shinji", "Kagawa");
            Assert.False(fullName1 != fullName2);

            var fullName3 = new FullName("Shinji", "Okazaki");
            Assert.True(fullName1 != fullName3);
        }
    }
}
