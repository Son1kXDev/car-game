using UnityEditor;
using UnityEngine;

namespace Assets.Game.Scripts.Data
{
    public interface ISettingsDataPersistence
    {
        void LoadData(SettingsData data);
        void SaveData(SettingsData data);
    }

}