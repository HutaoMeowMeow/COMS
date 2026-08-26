using Google.Cloud.Firestore;

namespace COMS.Data;

public interface IFirestoreRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> QueryAsync(Func<Query, Query>? queryBuilder = null);
    Task<string> CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
    Task<int> CountAsync(Func<Query, Query>? queryBuilder = null);
}

public class FirestoreRepository<T> : IFirestoreRepository<T> where T : class, new()
{
    protected readonly FirestoreDb _db;
    protected readonly string _collectionName;
    protected readonly Lazy<CollectionReference> _collection;

    public FirestoreRepository(FirestoreDb db, string collectionName)
    {
        _db = db;
        _collectionName = collectionName;
        _collection = new Lazy<CollectionReference>(() => _db.Collection(_collectionName));
    }

    public virtual async Task<T?> GetByIdAsync(string id)
    {
        var snapshot = await _collection.Value.Document(id).GetSnapshotAsync();
        if (!snapshot.Exists) return null;
        return FromDictionary(snapshot.ToDictionary());
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var snapshot = await _collection.Value.GetSnapshotAsync();
        return snapshot.Documents.Select(d => FromDictionary(d.ToDictionary())).ToList();
    }

    public virtual async Task<IEnumerable<T>> QueryAsync(Func<Query, Query>? queryBuilder = null)
    {
        Query query = _collection.Value;
        if (queryBuilder != null)
            query = queryBuilder(query);

        var snapshot = await query.GetSnapshotAsync();
        return snapshot.Documents.Select(d => FromDictionary(d.ToDictionary())).ToList();
    }

    public virtual async Task<string> CreateAsync(T entity)
    {
        var dict = ToDictionary(entity);
        var idProp = typeof(T).GetProperty("Id");
        string id;
        
        if (idProp != null && idProp.GetValue(entity) != null)
        {
            id = idProp.GetValue(entity)!.ToString()!;
            await _collection.Value.Document(id).SetAsync(dict);
        }
        else
        {
            var docRef = await _collection.Value.AddAsync(dict);
            id = docRef.Id;
            if (idProp != null && idProp.PropertyType == typeof(string))
                idProp.SetValue(entity, id);
        }
        return id;
    }

    public virtual async Task UpdateAsync(string id, T entity)
    {
        var dict = ToDictionary(entity);
        await _collection.Value.Document(id).SetAsync(dict, SetOptions.MergeAll);
    }

    public virtual async Task DeleteAsync(string id)
    {
        await _collection.Value.Document(id).DeleteAsync();
    }

    public virtual async Task<int> CountAsync(Func<Query, Query>? queryBuilder = null)
    {
        Query query = _collection.Value;
        if (queryBuilder != null)
            query = queryBuilder(query);

        var snapshot = await query.GetSnapshotAsync();
        return snapshot.Count;
    }

    protected virtual Dictionary<string, object?> ToDictionary(T entity)
    {
        var dict = new Dictionary<string, object?>();
        foreach (var prop in typeof(T).GetProperties())
        {
            var value = prop.GetValue(entity);
            dict[prop.Name] = ConvertToFirestoreValue(value);
        }
        return dict;
    }

    protected virtual T FromDictionary(Dictionary<string, object?> dict)
    {
        var entity = new T();
        foreach (var prop in typeof(T).GetProperties())
        {
            if (dict.TryGetValue(prop.Name, out var value) && value != null)
            {
                try
                {
                    var converted = ConvertFromFirestoreValue(value, prop.PropertyType);
                    prop.SetValue(entity, converted);
                }
                catch
                {
                    // Skip properties that can't be converted
                }
            }
        }
        return entity;
    }

    protected static object? ConvertToFirestoreValue(object? value)
    {
        if (value == null) return null;
        if (value is DateTime dt) return Timestamp.FromDateTime(dt.ToUniversalTime());
        if (value is Guid g) return g.ToString();
        if (value is IEnumerable<Guid> guids) return guids.Select(g => g.ToString()).ToList();
        if (value is IConvertible || value is IEnumerable<object>) return value;
        return value;
    }

    protected static object? ConvertFromFirestoreValue(object value, Type targetType)
    {
        if (value == null) return null;
        
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        
        if (underlyingType == typeof(Guid))
        {
            var str = value.ToString();
            return Guid.Parse(str ?? string.Empty);
        }
        if (underlyingType == typeof(Guid?))
        {
            var str = value.ToString();
            return string.IsNullOrEmpty(str) ? (Guid?)null : Guid.Parse(str);
        }
        if (underlyingType == typeof(DateTime))
        {
            if (value is Timestamp ts) return ts.ToDateTime();
            return Convert.ToDateTime(value);
        }
        if (underlyingType == typeof(DateTime?))
        {
            if (value is Timestamp ts) return (DateTime?)ts.ToDateTime();
            return Convert.ToDateTime(value);
        }
        
        return System.Convert.ChangeType(value, underlyingType);
    }
}
