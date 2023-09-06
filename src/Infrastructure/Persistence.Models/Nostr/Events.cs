namespace Nostrfi.Persistence.Models.Nostr;

/// <summary>
/// The only object that exists in the Nostr Protocol is an Event. An Event is a JSON object that contains a
///  public key, a signature, and a content object. The content object can be any JSON object. The signature is
///  a cryptographic signature of the content object. The public key is the public key that corresponds to the
///  https://github.com/nostr-protocol/nips/blob/master/01.md
/// </summary>
public class Events
{
    /// <summary>
    /// SHA256 hash of the content object, Serialized to JSON UTF-8 bytes, then base64 encoded.
    /// </summary>
    public string Id { get; set; }  = null!;
    public string PublicKey { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
   
    /// <summary>
    /// Specifies the type of event. The value of this field MUST be one of the following:
    ///  0 - Metadata : Content is set to stringified JSON object containing metadata about the feed, describing who created it, what it is for, etc.
    /// relays may delete older events once it gets a new one for the same PubKey.
    /// 1 - Text Note :  the content is set to the plaintext content of a note. Content that must be parsed, such as Markdown and HTML, should not be used. Clients should also not parse content as those.
    /// </summary>
    public int Kind { get; set; }
    public string? Content { get; set; }
    public string Signature { get; set; }
    public List<Tags> Tags { get; set; } = new();
    
}