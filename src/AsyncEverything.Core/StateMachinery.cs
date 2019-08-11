using System.Threading.Tasks;

namespace AsyncEverything.Core
{
    public class Foo
    {
        private int _data;

        public async Task<int> GetDataAsync()
        {
            if (_data != 0) return _data;

            _data = await FetchDataAsync();

            return _data;
        }

        public Task<int> FetchDataAsync()
        {
            return Task.FromResult(42);
        }
    }

    public class Foo_Deconstructed
    {
        private int _data;

        public Task<int> GetDataAsync()
        {
            var stateMachine = new BarAsyncStateMachine(this);
            stateMachine.Start();
            return stateMachine.Task;
        }

        public Task<int> FetchDataAsync()
        {
            return Task.FromResult(42);
        }

        class BarAsyncStateMachine
        {
            enum State { Start, State1 }

            private readonly Foo_Deconstructed _foo;
            private readonly TaskCompletionSource<int> _tcs = new TaskCompletionSource<int>();
            private State _state = State.Start;
            private Task<int> _fetchDataTask;

            public BarAsyncStateMachine(Foo_Deconstructed foo)
            {
                _foo = foo;
            }

            public void Start()
            {
                switch (_state)
                {
                    case State.Start:
                        if (_foo._data != 0)
                        {
                            _tcs.SetResult(_foo._data);

                            return;
                        }

                        _fetchDataTask = _foo.FetchDataAsync();

                        _state = State.State1;

                        _fetchDataTask.ContinueWith(_ => Start());

                        break;
                    case State.State1:
                        if (_fetchDataTask.IsCanceled) _tcs.SetCanceled();
                        else if (_fetchDataTask.IsFaulted) _tcs.SetException(_fetchDataTask.Exception);
                        else
                        {
                            _tcs.SetResult(_fetchDataTask.Result);
                        }

                        break;
                    default:
                        break;
                }
            }

            public Task<int> Task => _tcs.Task;
        }
    }


}
