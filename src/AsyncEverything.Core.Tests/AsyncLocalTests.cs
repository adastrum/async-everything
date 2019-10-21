using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncEverything.Core.Tests
{
    public class AsyncLocalTests
    {

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

        [Fact]
        public async Task AsyncLocalForCorrelationId()
        {
            var correlationManager = new CorrelationManager();

            async Task CrelateCorrelatedTask()
            {
                var correlationId = Guid.NewGuid().ToString();

                correlationManager.SetCorrelationId(correlationId);

                await Task.Delay(1000);

                Assert.Equal(correlationId, correlationManager.GetCorrelationId());
            }

            await Task.WhenAll(
                CrelateCorrelatedTask(),
                CrelateCorrelatedTask()
            );
        }
    }
}
