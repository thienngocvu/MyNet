namespace MyNet.Application.Common.Cache
{
    public interface ICache
    {
        Task StoreAsync<T>(string key, T value, int? database = null) where T : class;
        Task StoreAsync(string key, string value, int? database = null);
        Task StoreAsync<T>(string key, T value, TimeSpan duration, int? database = null) where T : class;
        Task StoreStringAsync(string key, string value, TimeSpan duration, int? database = null);
        Task<T> GetAsync<T>(string key, int? database = null) where T : class;
        Task<string> GetStringAsync(string key, int? database = null);
        Task<bool> DeleteAsync(string key, int? database = null);
        Task<bool> DeleteAllKeysAsync(string parternKey, int? database = null);
    }
}
