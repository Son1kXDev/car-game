using UnityEngine;
using UnityEditor;
using Utils.Debugger;

namespace Plugins
{
    public static class ClearPlayerPrefs
    {
        [MenuItem("Tools/Player Prefs/Clear Player Prefs")]
        private static void ClearPlayerPrefsMenuItem()
        {
            if (EditorUtility.DisplayDialog("Clear Player Prefs", "Are you sure you want to clear the player prefs?", "Yes", "Cansel", DialogOptOutDecisionType.ForThisSession, "Yes, don't ask me again"))
            {
                PlayerPrefs.DeleteAll();
                EditorUtility.DisplayDialog("Clear Player Prefs", "Player prefs was cleared successfully", "Ok");
            }
        }

        [MenuItem("Tools/Player Prefs/Player Prefs Delete Key")]
        private static void PlayerPrefsDeleteKeyMenuItem()
        {
            string KEY = Editors.TextEntryDialog.Show("PlayerPrefs", "Delete PlayerPrefs");
            {
                if (PlayerPrefs.HasKey(KEY))
                {
                    PlayerPrefs.DeleteKey(KEY);
                    EditorUtility.DisplayDialog("Delete player prefs key", $"Player prefs data for {KEY} was deleted", "Ok");
                }
            }
        }
    }
}