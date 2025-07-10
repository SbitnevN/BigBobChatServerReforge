using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BigBobChat.Infrastructure.Database.Repositories;

public abstract class DocumentBase
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
}
