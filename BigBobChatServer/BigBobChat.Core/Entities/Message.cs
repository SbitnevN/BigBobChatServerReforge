namespace BigBobChat.Core.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
}
