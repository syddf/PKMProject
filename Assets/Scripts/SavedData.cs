using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public enum EProgress
{
    New,
    FinishStory,
    FinishBattle1,
    FinishBattle2,
    FinishAllBattle
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    // 添加元素到字典
    public void Add(TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            keys.Add(key);
            values.Add(value);
            dictionary.Add(key, value);
        }
        else
        {
            Debug.LogWarning("Key already exists in dictionary: " + key);
        }
    }

    // 获取字典中的值
    public TValue GetValue(TKey key)
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            Debug.LogError("Key not found in dictionary: " + key);
            return default(TValue);
        }
    }

    // 实现ISerializationCallbackReceiver接口，用于在序列化和反序列化时同步keys和values
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary.Add(keys[i], values[i]);
        }
    }

    public bool ContainsKey(TKey InKey)
    {
        return dictionary.ContainsKey(InKey);
    }

    public TValue this[TKey key]
    {
        get { return GetValue(key); }
        set { dictionary[key] = value; }
    }

    public void Clear()
    {
        dictionary.Clear();
    }
}

[System.Serializable]
public struct BagPokemonOverrideData
{
    public bool Overrided;
    public string ReplaceTrainerName;
    public string ReplacePokemonName;
    public int SkillIndex0;
    public int SkillIndex1;
    public int SkillIndex2;
    public int SkillIndex3;
    public int SkillIndex4;
    public int SkillIndex5;
    public int SkillIndex6;
    public int SkillIndex7;
    public PokemonNature Nature;
    public int ItemIndex;
}

[System.Serializable]
public struct PlayerData
{
    public List<string> UseableTrainerList;
    public SerializableDictionary<string, SerializableDictionary<string, BagPokemonOverrideData>> OverrideData;
    public string BattleTrainerName;
    public List<EProgress> MainChapterProgress;
}

// 定义一个接口用于序列化和反序列化操作
public interface IDataSerializer
{
    void SerializeToFile<T>(T data, string filePath);
    T DeserializeFromFile<T>(string filePath);
}

public class EncryptedJSONDataSerializer : IDataSerializer
{
    // 加密密钥，可以根据需要自定义
    private const string encryptionKey = "5f4dcc3b5aa765d61d8327deb882cf99";

    public void SerializeToFile<T>(T data, string filePath)
    {
        string jsonData = JsonUtility.ToJson(data);
        string encryptedData = Encrypt(jsonData, encryptionKey);
        File.WriteAllText(filePath, encryptedData);
    }

    public T DeserializeFromFile<T>(string filePath)
    {
        string encryptedData = File.ReadAllText(filePath);
        string decryptedData = Decrypt(encryptedData, encryptionKey);
        return JsonUtility.FromJson<T>(decryptedData);
    }

    // 加密方法
    private string Encrypt(string plainText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.Mode = CipherMode.ECB; // ECB模式不推荐在实际项目中使用，此处仅作示例
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    encryptedBytes = msEncrypt.ToArray();
                }
            }
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    // 解密方法
    private string Decrypt(string cipherText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] decryptedBytes = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.Mode = CipherMode.ECB; // ECB模式不推荐在实际项目中使用，此处仅作示例
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream msPlainText = new MemoryStream())
                    {
                        csDecrypt.CopyTo(msPlainText);
                        decryptedBytes = msPlainText.ToArray();
                    }
                }
            }
        }

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}


public class SavedData : MonoBehaviour
{
    public PlayerData SavedPlayerData;
    public MapUI ReferenceMapUI;
    void OnApplicationQuit()
    {
        SaveData();
    }
    void Start()
    {
        string filePath = Application.dataPath + "/Saved/playerData.dat";
        // 定义文件路径
        if (File.Exists(filePath))
        {
            IDataSerializer serializer = new EncryptedJSONDataSerializer();
            PlayerData deserializedPlayerData = serializer.DeserializeFromFile<PlayerData>(filePath);
            SavedPlayerData = deserializedPlayerData;
        }
        else
        {
            PlayerData playerData = new PlayerData();
            List<string> UseableTrainerList = new List<string>();
            UseableTrainerList.Add("希特隆");
            UseableTrainerList.Add("可尔妮");
            UseableTrainerList.Add("帕琦拉");
            UseableTrainerList.Add("紫罗兰");
            UseableTrainerList.Add("查克洛");
            UseableTrainerList.Add("大吾");
            playerData.BattleTrainerName = "希特隆";
            playerData.UseableTrainerList = UseableTrainerList;

            playerData.OverrideData = new SerializableDictionary<string, SerializableDictionary<string, BagPokemonOverrideData>>();
            playerData.MainChapterProgress = new List<EProgress>();
            for(int Index = 0; Index <= 9; Index++)
            {
                playerData.MainChapterProgress.Add(EProgress.New);
            }
            playerData.MainChapterProgress[0] = EProgress.FinishAllBattle;
            IDataSerializer serializer = new EncryptedJSONDataSerializer();
            serializer.SerializeToFile(playerData, filePath);
            SavedPlayerData = playerData;
        }
        ReferenceMapUI.UpdateUI();

    }

    public void SaveData()
    {
        string filePath = Application.dataPath + "/Saved/playerData.dat";
        IDataSerializer serializer = new EncryptedJSONDataSerializer();
        serializer.SerializeToFile(SavedPlayerData, filePath);
    }
}
