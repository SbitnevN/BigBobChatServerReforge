using BigBobChat.Common.DependencyInjection;
using BigBobChat.Infrastructure.Database.Repositories.MessageRepository;
using BigBobChat.Infrastructure.Database.Repositories.UserRepository;
using BigBobChat.Infrastructure.Extensions;
using MongoDB.Driver;

namespace BigBobChat.Infrastructure.Database;

[Singleton(typeof(IDataBaseContext))]
public class DataBaseContext : IDataBaseContext
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;

    public IMongoCollection<UserDocument> Users { get; }

    public IMongoCollection<MessageDocument> Messages { get; }

    public DataBaseContext()
    {
        _client = new MongoClient("mongodb://localhost:27017/");
        _database = _client.GetDatabase("chat");

        Users = _database.GetCollection<UserDocument>(Users.GetName());
        Messages = _database.GetCollection<MessageDocument>(Messages.GetName());
    }
}
