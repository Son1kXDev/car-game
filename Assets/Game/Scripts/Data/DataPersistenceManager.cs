using System.ComponentModel;
using System.Collections;
using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Assets.Game.Scripts.Data
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }

        public static bool Loaded;

        [Header("Debug")]
        [SerializeField] private bool _initializeDataIfNull = false;

        [Header("Storage Config")]
        [SerializeField, StatusIcon("")] private string _settingsFileName;
        [SerializeField, StatusIcon("")] private string _gameFileName;
        [SerializeField] private bool _useEncryption = false;

        private GameData _gameData;
        private SettingsData _settingsData;
        private FileDataHandler _dataHandler;
        private List<IDataPersistence> _dataPersistenceObjects;
        private List<ISettingsDataPersistence> _settingsDataPersistenceObjects;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            Loaded = false;
            if (transform.parent != null) transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
            this._dataHandler = new FileDataHandler(Application.persistentDataPath, _gameFileName, _settingsFileName, _useEncryption);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this._dataPersistenceObjects = FindAllDataPersistanceObjects();
            this._settingsDataPersistenceObjects = FindAllSettingsDataPersistanceObjects();
            LoadGame();
            LoadSettings();
        }

        public bool SaveFileExist()
        {
            _gameData = _dataHandler.LoadGame();
            return _gameData != null;
        }

        public void NewGame()
        { _gameData = new GameData(); }

        public void LoadGame()
        {
            _gameData = _dataHandler.LoadGame();
            if (_gameData == null && _initializeDataIfNull) NewGame();
            if (_gameData == null) return;
            foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
                dataPersistence.LoadData(_gameData);
        }

        public void SaveGame()
        {
            if (_gameData == null) return;

            foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
                dataPersistence.SaveData(_gameData);

            _dataHandler.Save(_gameData);
        }

        public void ResetGame()
        {
            _dataHandler.Delete();
            _gameData = null;
        }

        public void NewSettings()
        { _settingsData = new SettingsData(); }

        public void LoadSettings()
        {
            _settingsData = _dataHandler.LoadSettings();
            if (_settingsData == null && _initializeDataIfNull) NewSettings();

            if (_settingsData == null) return;
            foreach (ISettingsDataPersistence dataPersistence in _settingsDataPersistenceObjects)
                dataPersistence.LoadData(_settingsData);

            Loaded = true;
        }

        public void SaveSettings()
        {
            if (_settingsData == null) return;

            foreach (ISettingsDataPersistence dataPersistence in _settingsDataPersistenceObjects)
                dataPersistence.SaveData(_settingsData);

            _dataHandler.Save(_settingsData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
            SaveSettings();
        }

        private List<IDataPersistence> FindAllDataPersistanceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        private List<ISettingsDataPersistence> FindAllSettingsDataPersistanceObjects()
        {
            IEnumerable<ISettingsDataPersistence> settingsDataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISettingsDataPersistence>();
            return new List<ISettingsDataPersistence>(settingsDataPersistenceObjects);
        }
    }
}