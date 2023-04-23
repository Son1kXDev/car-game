using System.IO;
using UnityEngine;
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
        string FileType = NativeFilePicker.ConvertExtensionToFileType("image/*");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null) success.Invoke(false);
            else
            {
                _path = path;
                SaveLoadedFile();
                callback.Invoke(LocalPath);
                success.Invoke(true);
                Debug.Log("Picked file " + path);
            }
        }, new string[] { FileType });

        Debug.Log(permission);
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