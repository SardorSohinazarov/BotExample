namespace Bot.Entities;

public class User
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public string FirstName { get; set; }
    public bool? IsBot { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? LanguageCode { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastInteractionAt { get; set; }
}
