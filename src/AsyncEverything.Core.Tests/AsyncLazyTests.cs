using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AsyncEverything.Core.Tests
{
    public class AsyncLazyTests
    {
        private Mock<IService<int>> _serviceMock;

        public AsyncLazyTests()
        {
            _serviceMock = new Mock<IService<int>>();
        }

        [Fact]
        public async Task HappyFlow()
        {
            //Arrange
            var expectedData = 42;
            _serviceMock.Setup(x => x.GetDataAsync()).ReturnsAsync(expectedData);
            var service = _serviceMock.Object;
            var asyncLazy = new AsyncLazy<int>(async () => await service.GetDataAsync());

            //Act
            var actualData = await asyncLazy;
            actualData = await asyncLazy;

            //Assert
            Assert.Equal(expectedData, actualData);
            _serviceMock.Verify(x => x.GetDataAsync(), Times.Once);
        }
    }
}
