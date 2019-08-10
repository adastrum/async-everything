using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsyncEverything.Core
{
    public class AsyncLazy<T> : Lazy<Task<T>>, INotifyCompletion
    {
        public AsyncLazy(Func<T> valueFactory)
            : base(() => Task.Run(valueFactory))
        {
        }

        public AsyncLazy(Func<Task<T>> valueFactory)
            : base(() => Task.Run(() => valueFactory()))
        {
        }

        public void OnCompleted(Action continuation) => continuation();

        public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();
    }
}
