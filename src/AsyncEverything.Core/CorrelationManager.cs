using System.Threading;

namespace AsyncEverything.Core
{
    public class CorrelationManager
    {
        private readonly AsyncLocal<string> _correlationId = new AsyncLocal<string>();
        //private readonly CorrelationId _correlationId = new CorrelationId();

        public string GetCorrelationId()
        {
            return _correlationId.Value;
        }

        public void SetCorrelationId(string value)
        {
            _correlationId.Value = value;
        }

        //private class CorrelationId
        //{
        //    public string Value { get; set; }
        //}
    }
}
