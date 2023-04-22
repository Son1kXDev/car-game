using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NTC.Global.Cache;


namespace Assets.Game.Scripts.Game
{
    public class MissionsManager : MonoCache
    {
        public List<Mission> _todaysMissions;
        public List<Mission> _missions;

        private int[] missionsID = { 0, 1, 2 };
        private bool missionsLoaded = false;

        private int drivedKM;
        private int currentCars;
        private int oldCars;

        private void Awake()
        {
            InitializeMissions();
        }

        private void Start()
        {
            StartCoroutine(DailyTimer());
        }

        private void InitializeMissions()
        {
            if (_missions.Count != 0) return;

            _missions = new List<Mission>
            {
                new("Drive 100 km.", 2100, (object o) => drivedKM >= 100),
                new("Buy new car", 5020, (object o) => currentCars > oldCars),
                new("dfgdfgdfg", 5040, (object o) => currentCars > oldCars),
                new("hjfghjdf", 51200, (object o) => currentCars > oldCars),
                new("346g bnddf", 500, (object o) => currentCars > oldCars),
                new("Lfkdf oasufh", 543500, (object o) => currentCars > oldCars),
                new("fhg dfbdh sdfbdf dfdhfbdf d dfd df", 5323400, (object o) => currentCars > oldCars),
                new("134g fgf45 bfg645", 234, (object o) => currentCars > oldCars),
            };
            LoadDailyMissions();
        }

        private void GenerateNewDailyMissions()
        {
            Debug.Log("Generating new daily missions");

            int[] numbers = Enumerable.Range(0, _missions.Count - 1).ToArray();
            DistinctRandom missions = new(numbers);

            _todaysMissions.Clear();
            for (int i = 0; i < 3; i++)
            {
                missionsID[i] = missions.Next();
                _todaysMissions.Add(_missions[missionsID[i]]);
            }
            missionsLoaded = true;
        }

        private void LoadDailyMissions()
        {
            if (missionsLoaded) return;

            Debug.Log("Loading daily missions");

            for (int i = 0; i < 3; i++)
            {
                _todaysMissions.Add(_missions[missionsID[i]]);
            }
            missionsLoaded = true;
        }

        protected override void Run()
        {
            CheckCompletion();
        }

        private IEnumerator DailyTimer()
        {
            while (true)
            {
                if (NewDay())
                {
                    PlayerPrefs.SetString("LAST_OPEN_TIME", DateTime.Now.ToString("yyyy-MM-dd"));
                    GenerateNewDailyMissions();
                }
                yield return null;
            }
        }

        private bool NewDay()
        {
            string lastOpenedTime = PlayerPrefs.GetString("LAST_OPEN_TIME", DateTime.Now.ToString("yyyy-MM-dd"));
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            if (updateDate)
            {
                updateDate = false;
                return true;
            }
            return lastOpenedTime != currentTime;
        }

        private bool updateDate = false;

        [ContextMenu("Update Timer")]
        public void UpdateDay()
        {
            updateDate = true;
        }

        private void CheckCompletion()
        {
            if (_todaysMissions == null) return;

            _todaysMissions.ForEach(m => m.UpdateCompletion());
        }
    }

    [System.Serializable]
    public class Mission
    {
        public string Description;
        public int Reward;
        public Predicate<object> Requirement;

        public bool Completed;

        public Mission(string description, int reward, Predicate<object> requirement)
        {
            Description = description;
            Reward = reward;
            Requirement = requirement;
        }

        public void UpdateCompletion()
        {
            if (Completed) return;

            if (RequirementsMet())
            {
                Completed = true;
            }
        }

        public bool RequirementsMet()
        {
            return Requirement.Invoke(null);
        }
    }
}