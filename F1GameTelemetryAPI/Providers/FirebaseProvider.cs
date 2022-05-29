namespace F1GameTelemetryAPI.Providers;

using FireSharp;
using Interfaces;
using System.Text.Json;

public class FirebaseProvider : IFirebaseProvider
{
    private readonly FirebaseClient _firebaseDb = null!;

    public FirebaseProvider(FirebaseClient fireStoreDb)
    {
        _firebaseDb = fireStoreDb;
    }

    public async Task AddOrUpdate<T>(string endpoint, T data) where T : IFirebaseEntity
    {
        await _firebaseDb.SetAsync(endpoint, data);
    }

    public async Task<T> Get<T>(string endpoint, string id) where T : IFirebaseEntity
    {
        var document = await _firebaseDb.GetAsync($"{endpoint}/{id}");
        return (T)JsonSerializer.Deserialize(document.Body, typeof(T))!;
    }

    public async Task<IReadOnlyCollection<T>> GetAll<T>(string endpoint) where T : IFirebaseEntity
    {
        var collection = await _firebaseDb.GetAsync(endpoint);
        var d = (Dictionary<string, T>)JsonSerializer.Deserialize(collection.Body, typeof(Dictionary<string, T>))!;
        if (d == null)
            return new List<T>();
        return d.Values.ToList();
    }

    // just add here any method you need here WhereEqualTo, WhereGreaterThan, WhereIn etc ...

}

