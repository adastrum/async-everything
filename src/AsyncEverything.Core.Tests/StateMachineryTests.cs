using System;
using System.Threading.Tasks;
using Xunit;

namespace AsyncEverything.Core.Tests
{
    public class StateMachineryTests
    {
        [Fact]
        public async Task HappyFlow()
        {
            var foo1 = new Foo();
            var data1 = await foo1.GetDataAsync();

            var foo2 = new Foo_Deconstructed();
            var data2 = await foo2.GetDataAsync();

            Assert.Equal(data1, data2);
        }
    }
}
