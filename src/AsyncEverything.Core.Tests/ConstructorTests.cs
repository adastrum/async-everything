using System;
using System.Threading.Tasks;
using Xunit;

namespace AsyncEverything.Core.Tests
{
    public class ConstructorTests
    {
        [Fact]
        public async Task FactoryMethod()
        {
            var bar = await Bar.CreateAsync();
        }

        [Fact]
        public async Task AsyncInitialization()
        {
            //var baz = new Baz();
            var baz = Activator.CreateInstance<Baz>();

            await baz.Initialization;
        }
    }
}
