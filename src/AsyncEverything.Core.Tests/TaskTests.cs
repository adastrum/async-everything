using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AsyncEverything.Core.Tests
{
    public class TaskTests
    {
        [Fact]
        public void ExceptionHandling()
        {
            Assert.Throws<CustomException>(() => CreateTaskThatThrows().GetAwaiter().GetResult());

            Assert.Throws<AggregateException>(() => CreateTaskThatThrows().Wait());
        }

        [Fact]
        public async Task AsyncVoid()
        {
            Func<Task<int>> provider = () => CreateTaskThatThrows<int>();
            //Task<int> provider() => CreateTaskThatThrows<int>();

            //Lambda expression results in async void when converted to Action<T>
            //It becomes obvious if converted to local function

            Action<Exception> onError = async (e) => { Console.WriteLine(e.Message); };
            //async void onError(Exception e) { Console.WriteLine(e.Message); }

            await ActionWithRetry(provider, onError);
        }

        private Task CreateTaskThatThrows() => Task.Run(() => throw new CustomException());

        private Task<T> CreateTaskThatThrows<T>()
        {
            return Task.Run(() =>
            {
                throw new CustomException();

                return default(T);
            });
        }

        private Task<T> ActionWithRetry<T>(Func<Task<T>> provider, Action<Exception> onError)
        {
            return provider().ContinueWith((t) =>
            {
                if (t.Status == TaskStatus.Faulted)
                {
                    onError(t.Exception);

                    return default;
                }

                return t.Result;
            });
        }
    }
}
