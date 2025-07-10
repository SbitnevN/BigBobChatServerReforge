using BigBobChat.Infrastructure.Database.Repositories;

namespace BigBobChat.Infrastructure.Database.Repositories.UserRepository;

public class UserDocument : DocumentBase
{
    public string Name { get; set; }

    public string Email { get; set; }
}
