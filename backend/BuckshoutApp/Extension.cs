using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BuckshoutApp
{
    public static class Extension
    {

        public static T? Pop<T>(this IList<T> list)
        {
            if (list.Count < 1) return default;
            var item = list.Last();
            list.Remove(item);
            return item;
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
        public static T RandomChoise<T>(this IList<T> list)
        {
            var random = new Random();
            return list[random.Next(list.Count)];
        }
    }
    public static class TimerExtension
    {

        private static Dictionary<int, System.Timers.Timer> _timers = [];
        private static int _nextTimerId = 1;
        public static int SetInterval(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.Enabled = true;
            timer.Start();

            int timerId = _nextTimerId++;
            _timers[timerId] = timer;

            return timerId;
        }

        public static IDisposable SetTimeout(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();

            return timer;
        }
        public static void ClearInterval(int timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.Stop();
                timer.Dispose();
                _timers.Remove(timerId);
            }
        }
    }

    public static class NetworkUtils
    {
        public static string GetLocalIPv4(NetworkInterfaceType type)
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == type && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    var ipProperties = networkInterface.GetIPProperties();
                    var unicastAddress = ipProperties.UnicastAddresses.FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork);
                    if (unicastAddress != null)
                    {
                        return unicastAddress.Address.ToString();
                    }
                }
            }
            return null;
        }
    }
}
