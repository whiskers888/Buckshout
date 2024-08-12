using BuckshoutApp.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp
{
    public static class Extension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    public static class TimerExtension
    {

        private static Dictionary<int, System.Timers.Timer> _timers = new Dictionary<int, System.Timers.Timer>();
        private static int _nextTimerId = 1;
        public static int SetInterval(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
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
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();

            return timer as IDisposable;
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
}
