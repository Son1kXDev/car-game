using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins
{
    public class Data : MonoBehaviour
    {
        private static Data _instance;

        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Data();
                }

                return _instance;
            }
        }

        public string TopScore
        {
            get { return PlayerPrefs.GetString("TopScore", "0"); }
            set
            {
                PlayerPrefs.SetString("TopScore", value);
                PlayerPrefs.Save();
            }
        }

        public int SubIndex
        {
            get => PlayerPrefs.GetInt($"Index", 0);
            set => PlayerPrefs.SetInt($"Index", value);
        }

        public long TicksPerMonth()
        {
            return TimeSpan.TicksPerDay * 30;
        }

        public long TicksPerWeek()
        {
            return TimeSpan.TicksPerDay * 7;
        }

        public long TicksPerTwoWeek()
        {
            return TimeSpan.TicksPerDay * 14;
        }

        public class Subscription
        {
            public int Index;

            public string Name
            {
                get => PlayerPrefs.GetString($"SubName_{Index}", string.Empty);
                set => PlayerPrefs.SetString($"SubName_{Index}", value);
            }

            public string FirstPay
            {
                get => PlayerPrefs.GetString($"FirstPay_{Index}", string.Empty);
                set => PlayerPrefs.SetString($"FirstPay_{Index}", value);
            }

            public int Period
            {
                get => PlayerPrefs.GetInt($"Period_{Index}", 0);
                set => PlayerPrefs.SetInt($"Period_{Index}", value);
            }

            public bool IsDeleted
            {
                get => bool.Parse(PlayerPrefs.GetString($"IsDeleted_{Index}", "false"));
                set => PlayerPrefs.SetString($"IsDeleted_{Index}", value.ToString());
            }

            public string Cost
            {
                get => PlayerPrefs.GetString($"Cost_{Index}", string.Empty);
                set => PlayerPrefs.SetString($"Cost_{Index}", value);
            }
            public bool IsShowNotification
            {
                get => bool.Parse(PlayerPrefs.GetString($"IsShowNotification_{Index}", "true"));
                set => PlayerPrefs.SetString($"IsShowNotification_{Index}", value.ToString());
            }

            public Subscription(int index, string name, string firstPay, int period, string cost, bool isDeleted)
            {
                Index = index;
                Name = name;
                FirstPay = firstPay;
                Period = period;
                Cost = cost;
                IsDeleted = isDeleted;
            }

            public Subscription(int index)
            {
                Index = index;
            }
        }

        public List<Subscription> GetCurrentSubList()
        {
            var tempList = new List<Subscription>();
            var subCount = _instance.SubIndex;
            for (int i = 0; i < subCount; i++)
            {
                var init = new Subscription(i);
                if (!init.IsDeleted)
                    tempList.Add(init);
            }

            return tempList;
        }

        public int DaysFromTicks(long ticks)
        {
            return (int)(ticks / TimeSpan.TicksPerDay);
        }

        public Subscription selectedSubscription;

        public bool Sound
        {
            get
            {
                var result = bool.Parse(PlayerPrefs.GetString("Sound", "true"));
                return result;
            }
            set
            {
                PlayerPrefs.SetString("Sound", value.ToString());
                PlayerPrefs.Save();
            }
        }

        public bool Music
        {
            get
            {
                var result = bool.Parse(PlayerPrefs.GetString("Music", "true"));
                return result;
            }
            set
            {
                PlayerPrefs.SetString("Music", value.ToString());
                PlayerPrefs.Save();
            }
        }
    }
}