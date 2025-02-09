using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    public static void Save(float completeTime) {
        Data data = Load();

        if(completeTime == 0) {
            if (data != null) completeTime = data.CompleteTime;
        }
        else {
            if (completeTime > data.CompleteTime && data.CompleteTime != 0) {
                if (data != null) completeTime = data.CompleteTime;
            }
        }

        string path = Application.persistentDataPath + "/Data.bi";
        FileStream stream = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new();
        formatter.Serialize(stream, new Data(completeTime, UIManager.Instance.GetSoundEffectVolum(), UIManager.Instance.GetMusicVolum()));
        stream.Close();
    }
    public static Data Load() {
        string path = Application.persistentDataPath + "/Data.bi";
        if (File.Exists(path)) {
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new();
            Data data = (Data)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else {
            return null;
        }
    }
}
[System.Serializable]
public class Data {
    public float CompleteTime;
    public float SoundEffectVolum;
    public float MusicVolum;
    public Data(float completeTime,float soundEffectVolum,float musicVolum) {
        CompleteTime = completeTime;
        SoundEffectVolum = soundEffectVolum;
        MusicVolum = musicVolum;
    }
}
