using System.Threading.Tasks;

namespace AsyncEverything.Core.Tests
{
    /// <summary>
    /// For testing purposes only
    /// </summary>
    public interface IService<T>
    {
        Task<T> GetDataAsync();
    }
}
