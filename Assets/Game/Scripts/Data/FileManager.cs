using System.IO;
using UnityEngine;
#if !UNITY_ANDROID
using AnotherFileBrowser.Windows;
#endif
using Utils.Debugger;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance { get; private set; }

    public string LocalPath;
    private string _path;

    void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;
    }

    public void LoadFile(System.Action<string> callback, System.Action<bool> success)
    {
#if UNITY_ANDROID
        string FileType = "image/*";

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null) success.Invoke(false);
            else
            {
                _path = path;
                SaveLoadedFile();
                callback.Invoke(LocalPath);
                success.Invoke(true);
            }
        }, new string[] { FileType });
#else
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            _path = path;
            SaveLoadedFile();
            callback.Invoke(LocalPath);
            success.Invoke(true);
        });
#endif
    }

    public void SaveLoadedFile()
    {
        string fileExtension = Path.GetExtension(_path);
        LocalPath = Path.Combine(Application.persistentDataPath, "sticker" + fileExtension);
        PlayerPrefs.SetString("StickerExtension", fileExtension);
        var bytes = File.ReadAllBytes(_path);
        File.WriteAllBytes(LocalPath, bytes);
    }

}