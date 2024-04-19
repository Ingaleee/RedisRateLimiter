using HomeworkApp.Bll.Services;
using HomeworkApp.Bll.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HomeworkApp.IntegrationTests.ServicesTests
{
    public class RateLimiterServiceTests
    {
        [Fact]
        public async Task Should_Allow_Request_When_Under_Limit()
        {
            var cacheMock = new Mock<ICacheService>();
            cacheMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync("10");

            var service = new RateLimiterService(cacheMock.Object);
            var result = await service.IsRequestAllowed("127.0.0.1");

            Assert.True(result);
        }

        [Fact]
        public async Task Should_Not_Allow_Request_When_Over_Limit()
        {
            var cacheMock = new Mock<ICacheService>();
            cacheMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync("100");

            var service = new RateLimiterService(cacheMock.Object);
            var result = await service.IsRequestAllowed("127.0.0.1");

            Assert.False(result);
        }
    }
}
