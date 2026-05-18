using System.Linq.Expressions;
using MongoDB.Driver;

namespace BuildingBlocks.Persistence.Mongo;

public interface IMongoRepository<TDocument> where TDocument : class
{
    Task<TDocument?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<TDocument>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, CancellationToken ct = default);
    Task InsertAsync(TDocument document, CancellationToken ct = default);
    Task UpdateAsync(string id, TDocument document, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
}

public sealed class MongoRepository<TDocument>(IMongoCollection<TDocument> collection)
    : IMongoRepository<TDocument> where TDocument : class
{
    public async Task<TDocument?> GetByIdAsync(string id, CancellationToken ct = default) =>
        await collection.Find(Builders<TDocument>.Filter.Eq("_id", id)).FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<TDocument>> GetAllAsync(CancellationToken ct = default) =>
        await collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync(ct);

    public async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, CancellationToken ct = default) =>
        await collection.Find(filter).ToListAsync(ct);

    public Task InsertAsync(TDocument document, CancellationToken ct = default) =>
        collection.InsertOneAsync(document, cancellationToken: ct);

    public Task UpdateAsync(string id, TDocument document, CancellationToken ct = default) =>
        collection.ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", id), document, cancellationToken: ct);

    public Task DeleteAsync(string id, CancellationToken ct = default) =>
        collection.DeleteOneAsync(Builders<TDocument>.Filter.Eq("_id", id), ct);
}
