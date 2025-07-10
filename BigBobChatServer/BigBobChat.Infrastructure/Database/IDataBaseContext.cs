using BigBobChat.Infrastructure.Database.Repositories.MessageRepository;
using BigBobChat.Infrastructure.Database.Repositories.UserRepository;
using MongoDB.Driver;

namespace BigBobChat.Infrastructure.Database
{
    public interface IDataBaseContext
    {
        IMongoCollection<MessageDocument> Messages { get; }
        IMongoCollection<UserDocument> Users { get; }
    }
}