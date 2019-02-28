using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DddBase.Tests
{
    public class InMemoryRepositoryTest
    {
        public class TestEntity : Entity<Guid>
        {
            public TestEntity(Guid id)
                : base(id)
            {
            }
        }

        [Fact]
        public async Task ResolveTest()
        {
            var repository = new InMemoryRepository<TestEntity, Guid>();
            var expected = new TestEntity(new Guid("E5318031-FF08-47A3-A35D-EE8F56B64C67"));
            await repository.StoreAsync(expected);

            var actual = await repository.ResolveAsync(new Guid("E5318031-FF08-47A3-A35D-EE8F56B64C67"));
            Assert.True(ReferenceEquals(expected, actual));
            Assert.Equal(expected.Id, actual.Id);

            Assert.Null(await repository.ResolveAsync(new Guid("CF85F4DC-1766-4EB8-BDBB-4B29872D9479")));
        }
    }
}
