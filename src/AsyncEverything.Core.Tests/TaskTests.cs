using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncEverything.Core.Tests
{
    public class TaskTests
    {

        [Fact]
        public void ExceptionHandling()
        {
            Assert.Throws<CustomException>(() => CreateTask().GetAwaiter().GetResult());

            Assert.Throws<AggregateException>(() => CreateTask().Wait());
        }

        [Fact]
        public async Task AsyncLocalVsThreadLocal()
        {
            var value = 42;

            var al = new AsyncLocal<int>
            {
                Value = value
            };

            var tl = new ThreadLocal<int>
            {
                Value = value
            };

            await Task.Run(() =>
            {
                Assert.Equal(value, al.Value);
                Assert.Equal(0, tl.Value);
            })
            .ContinueWith((previousTask) => Task.Run(() =>
            {
                Assert.Equal(value, al.Value);
                Assert.Equal(0, tl.Value);
            }), TaskContinuationOptions.RunContinuationsAsynchronously);
        }

        private Task CreateTask() => Task.Run(() => throw new CustomException());
    }
}
