namespace F1GameTelemetryAPI.Providers;

using Interfaces;

public interface IFirestoreProvider
{
    Task AddOrUpdate<T>(T entity, CancellationToken ct) where T : IFirebaseEntity;
    Task<T> Get<T>(string id, CancellationToken ct) where T : IFirebaseEntity;
    Task<IReadOnlyCollection<T>> GetAll<T>(CancellationToken ct) where T : IFirebaseEntity;
    Task<IReadOnlyCollection<T>> WhereEqualTo<T>(string fieldPath, object value, CancellationToken ct) where T : IFirebaseEntity;
}
