namespace Buckshout.Models
{
    public record UserConnection(string connectionId, string? playerName = null, string? roomName = null);
}
