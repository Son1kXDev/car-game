using System.Collections;
using UnityEngine;
using Utils.Debugger;
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
        [SerializeField] private string _fileName;
        [SerializeField] private bool _useEncryption = false;

        private GameData _gameData;
        private FileDataHandler _dataHandler;
        private List<IDataPersistence> _dataPersistenceObjects;

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
            this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
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
            LoadGame();
        }

        public bool SaveFileExist()
        {
            _gameData = _dataHandler.Load();
            return _gameData != null;
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load();
            if (_gameData == null && _initializeDataIfNull) NewGame();
            if (_gameData == null) return;
            foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
                dataPersistence.LoadData(_gameData);

            Loaded = true;
        }

        [ContextMenu("Save")]
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

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistanceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}