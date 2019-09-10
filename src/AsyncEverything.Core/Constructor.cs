using System.Threading.Tasks;

namespace AsyncEverything.Core
{
    public class Bar
    {
        private Bar()
        {

        }

        private async Task<Bar> InitializeAsync()
        {
            await Task.Yield();

            return this;
        }

        public static async Task<Bar> CreateAsync()
        {
            var result = new Bar();

            await result.InitializeAsync();

            return result;
        }
    }

    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }

    public class Baz : IAsyncInitialization
    {
        public Baz()
        {
            Initialization = InitializeAsync();
        }

        public Task Initialization { get; }

        private async Task InitializeAsync()
        {
            await Task.Yield();
        }
    }
}
