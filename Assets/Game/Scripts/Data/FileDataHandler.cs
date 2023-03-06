using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string _dataDirectoryPath = "";
    private string _dataFileName = "";
    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord = "empire";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool useEncryption)
    {
        _dataDirectoryPath = dataDirectoryPath;
        _dataFileName = dataFileName;
        _useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (_useEncryption) dataToLoad = EncryptDecrypt(dataToLoad);
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            { Utils.Debugger.Console.LogError("Error while trying load data from file: " + fullPath + "\n" + e); }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_useEncryption) dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        { Utils.Debugger.Console.LogError("Error while trying saving data to file: " + fullPath + "\n" + e); }
    }

    public void Delete()
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _dataFileName);
        if (File.Exists(fullPath))
        {
            try
            { File.Delete(fullPath); }
            catch (Exception e)
            { Utils.Debugger.Console.LogError("Error while trying delete data from file: " + fullPath + "\n" + e); }
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        return modifiedData;
    }
}