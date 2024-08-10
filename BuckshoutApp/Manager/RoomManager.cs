namespace Buckshout.Managers
{
    //TODO:  Здесь должна быть логика комнат для выбора их из списка на фронте, но это не сработает в данный момент, потому что класс не реализован для редиса.
    public class RoomManager
    {
        private static readonly Dictionary<string, List<string>> room = new Dictionary<string, List<string>>();

        public static void AddToRoom(string roomName, string connectionId)
        {
            if (!room.ContainsKey(roomName))
            {
                room[roomName] = new List<string>();
            }
            room[roomName].Add(connectionId);
        }

        public static void RemoveFromRoom(string roomName, string connectionId)
        {
            if (room.ContainsKey(roomName))
            {
                room[roomName].Remove(connectionId);
                if (room[roomName].Count == 0)
                {
                    room.Remove(roomName);
                }
            }
        }
        public static string GetRoom(string roomName)
        {
            return room.FirstOrDefault(it => it.Key == roomName).Key;
        }
        public static List<string> GetAllRooms()
        {
            return room.Keys.ToList();
        }
    }
}
