﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DddBase.Repositories;
using Xunit;

namespace DddBase.Tests
{
    public class InMemoryRepositoryTest
    {
        public class TestAggregate : IAggregateRoot<Guid>
        {
            public TestAggregate(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }
        }

        [Fact]
        public async Task ResolveAsyncTest()
        {
            var repository = new InMemoryRepository<TestAggregate, Guid>();
            var expected = new TestAggregate(new Guid("E5318031-FF08-47A3-A35D-EE8F56B64C67"));
            await repository.StoreAsync(expected);

            var actual = await repository.ResolveAsync(new Guid("E5318031-FF08-47A3-A35D-EE8F56B64C67"));
            Assert.True(ReferenceEquals(expected, actual));
            Assert.Equal(expected.Id, actual.Id);

            Assert.Null(await repository.ResolveAsync(new Guid("CF85F4DC-1766-4EB8-BDBB-4B29872D9479")));
        }
    }
}
