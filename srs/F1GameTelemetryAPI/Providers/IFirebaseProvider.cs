namespace F1GameTelemetryAPI.Providers;

using Interfaces;

public interface IFirebaseProvider
{
    Task AddOrUpdate<T>(string endpoint, T data) where T : IFirebaseEntity;
    Task<T> Get<T>(string endpoint, string id) where T : IFirebaseEntity;
    Task<IReadOnlyCollection<T>> GetAll<T>(string endpoint) where T : IFirebaseEntity;
}
