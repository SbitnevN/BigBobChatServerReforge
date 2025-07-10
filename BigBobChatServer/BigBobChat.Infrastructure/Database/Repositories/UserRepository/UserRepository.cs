using BigBobChat.Common.DependencyInjection;
using BigBobChat.Core.Entities;
using Mapster;
using MongoDB.Driver;

namespace BigBobChat.Infrastructure.Database.Repositories.UserRepository;

[Singleton(typeof(UserRepository))]
public class UserRepository(IDataBaseContext context)
{
    private readonly IMongoCollection<UserDocument> _usersCollection = context.Users;

    public async Task<IEnumerable<User>> GetAll()
    {
        IEnumerable<UserDocument> documents = await _usersCollection.Find(_ => true).ToListAsync();
        return documents.Adapt<List<User>>();
    }
}