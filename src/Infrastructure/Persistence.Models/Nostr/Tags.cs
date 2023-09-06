namespace Nostrfi.Persistence.Models.Nostr;

public class Tags
{
    public string Id { get; set; }
    public string EventId { get; set; }
    public Events Event { get; set; } = null!;
    public string Tag { get; set; }
    public string? Value { get; set; }
    
   
}