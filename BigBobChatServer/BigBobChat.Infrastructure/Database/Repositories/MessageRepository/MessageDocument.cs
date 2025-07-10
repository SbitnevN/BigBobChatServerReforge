namespace BigBobChat.Infrastructure.Database.Repositories.MessageRepository;

public class MessageDocument : DocumentBase
{
    public string Content { get; set; }
    public DateTime Date { get; set; }
}
