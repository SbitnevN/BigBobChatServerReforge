using BigBobChat.Core.Extensions;
using BigBobChat.Infrastructure.Database.Documents;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace BigBobChat.Infrastructure.Extensions;

public static class MongoCollectionExtensions
{
    public static string GetName<T>(
        this IMongoCollection<T>? collection,
        [CallerArgumentExpression(nameof(collection))] string? callerExpression = null)
    {
        string? name = callerExpression?.Split('.').Last();
        return name?.ToSnakeCase() ?? collection.GetType().Name.ToSnakeCase();
    }
}
