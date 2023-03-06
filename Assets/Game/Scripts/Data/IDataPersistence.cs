using UnityEditor;
using UnityEngine;

namespace Assets.Game.Scripts.Data
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);

        void SaveData(GameData data);
    }
}