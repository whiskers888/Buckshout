namespace Buckshout.Hubs
{
    public enum MessageType
    {
        ReceiveMessage,
        ExceptionMessage,
        RoomCreated,
        GameStarted,
        RoundStarted,
        Damage,
        Health,
        Shoot,
        MovePass
    }

    public static class MessageTypeExtensions
    {
        public static string ToStr(this MessageType messageType)
        {
            return messageType.ToString();
        }
    }
}
