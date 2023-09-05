namespace Persistence.Models;

public class Events
{
    public string Id { get; set; }
    public string PublicKey { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public int Kind { get; set; }
    public string? Content { get; set; }
    public string Signature { get; set; }
    public List<Tags> Tags { get; set; } = new();
    
}