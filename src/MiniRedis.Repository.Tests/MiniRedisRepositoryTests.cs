namespace MiniRedis.Repository.Tests
{
    using System;
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniRedis.Repository.Implementations;
    using MiniRedis.Repository.Models;

    [TestClass]
    public class MiniRedisRepositoryTests
    {
        private ICacheRepository<CacheItemModel> repository;
        private Fixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            this.repository = new MiniRedisRepository();
            this.fixture = new Fixture();
        }

        [TestMethod]
        public void Set_ValidKeyAndValue_NoExceptionRaised()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            var value = this.fixture.Create<CacheItemModel>();

            // Act
            Action act = () => this.repository.AddOrUpdate(key, value);

            // Assert
            act.Should().NotThrow();
        }

        [TestMethod]
        public void Get_ValidKey_NullReturned()
        {
            // Arrange
            const string key = "ValidKeyAndValue";

            // Act
            var result = this.repository.Get(key);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Count_EmptyCache_ZeroReturned()
        {
            // Act
            var result = this.repository.Count();

            // Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void Delete_InexistentKey_FalseReturned()
        {
            // Arrange
            const string key = "ValidKeyAndValue";

            // Act
            var result = this.repository.Delete(key);

            // Assert
            result.Should().BeFalse();
        }
    }
}
