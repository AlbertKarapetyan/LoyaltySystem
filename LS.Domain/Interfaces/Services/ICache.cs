namespace LS.Domain.Interfaces.Services
{
    public interface ICache
    {
        Task SetValueAsync(string key, string value);
        Task<string?> GetValueAsync(string key);
    }

}
