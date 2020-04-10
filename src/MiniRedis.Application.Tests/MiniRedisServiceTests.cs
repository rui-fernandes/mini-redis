namespace MiniRedis.Application.Tests
{
    using System;
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniRedis.Application.Implementations;
    using MiniRedis.Repository;
    using MiniRedis.Repository.Models;
    using Moq;

    [TestClass]
    public class MiniRedisServiceTests
    {
        private Mock<ICacheRepository<CacheItemModel>> repositoryMock;
        private ICacheService service;
        private Fixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            this.repositoryMock = new Mock<ICacheRepository<CacheItemModel>>();
            this.service = new MiniRedisService(this.repositoryMock.Object);
            this.fixture = new Fixture();
        }

        [TestMethod]
        public void Set_ValidKeyAndValue_NoExceptionRaised()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            const string value = "value";

            this.repositoryMock
                .Setup(r => r.AddOrUpdate(key, It.Is<CacheItemModel>(m => m.Value == value)));

            // Act
            Action act = () => this.service.Set(key, null, value);

            // Assert
            act.Should().NotThrow();
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Set_ValidKeyValueWithSeconds_NoExceptionRaised()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            const string value = "value";
            const int seconds = 10;

            this.repositoryMock
                .Setup(r => r.AddOrUpdate(key, It.IsAny<CacheItemModel>()));

            // Act
            Action act = () => this.service.Set(key, seconds, value);

            // Assert
            act.Should().NotThrow();
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ValidKey_ItemReturned()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            var value = this.fixture
                .Build<CacheItemModel>()
                .With(m => m.ExpiresAt, DateTime.UtcNow.AddSeconds(120))
                .Create();

            this.repositoryMock
                .Setup(r => r.Get(key))
                .Returns(value);

            // Act
            var result = this.service.Get(key);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(value.Value);
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Count_NumberOfItemsReturned()
        {
            // Arrange
            const int itemsCount = 10;

            this.repositoryMock
                .Setup(r => r.Count())
                .Returns(itemsCount);

            // Act
            var result = this.service.GetSize();

            // Assert
            result.Should().Be(itemsCount);
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Delete_ValidKey_ItemDeleted()
        {
            // Arrange
            const string key = "ValidKeyAndValue";

            this.repositoryMock
                .Setup(r => r.Delete(key))
                .Returns(true);

            // Act
            var result = this.service.Delete(key);

            // Assert
            result.Should().BeTrue();
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Increment_ValidKey_ValueIncremented()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            const string value = "1";
            var model = this.fixture
                .Build<CacheItemModel>()
                .With(m => m.Value, value)
                .Create();

            this.repositoryMock
                .Setup(r => r.Get(key))
                .Returns(model);

            this.repositoryMock
                .Setup(r => r.AddOrUpdate(
                    key,
                    It.Is<CacheItemModel>(m => int.Parse(m.Value) == int.Parse(value) + 1)));

            // Act
            Action act = () => this.service.Increment(key);

            // Assert
            act.Should().NotThrow();
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void SortedSet_ValidScoreAndMember_NoExceptionRaised()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            const int score = 1;
            const string member = "member";

            this.repositoryMock
                .Setup(r => r.AddOrUpdate(key, It.IsAny<CacheItemModel>()));

            // Act
            Action act = () => this.service.SortedSet(key, score, member);

            // Assert
            act.Should().NotThrow();
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void SortedCardinality_ValidKey_CardinalityReturned()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            const string value = "1-one.2-two";
            const int expectedCardinality = 2;
            var model = this.fixture
                .Build<CacheItemModel>()
                .With(m => m.Value, value)
                .Create();

            this.repositoryMock
                .Setup(r => r.Get(key))
                .Returns(model);

            // Act
            var result = this.service.SortedCardinality(key);

            // Assert
            result.Should().Be(expectedCardinality);
            this.repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void RankMember_ValidKeyAndMember_RankReturned()
        {
            // Arrange
            const string key = "ValidKeyAndValue";
            const string value = "1-one.2-two";
            const int rankExpected = 0;
            const string member = "one";
            var model = this.fixture
                .Build<CacheItemModel>()
                .With(m => m.Value, value)
                .Create();

            this.repositoryMock
                .Setup(r => r.Get(key))
                .Returns(model);

            // Act
            var result = this.service.RankMember(key, member);

            // Assert
            result.Should().Be(rankExpected);
            this.repositoryMock.VerifyAll();
        }
    }
}
