using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRedisService
    {
        Task<T> GetItemAsync<T>(string key);
        Task SetItemAsync<T>(string key, T value, int expirySeconds);
        void Connect();
    }
}
