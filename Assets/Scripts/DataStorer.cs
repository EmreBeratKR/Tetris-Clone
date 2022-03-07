using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStorer : MonoBehaviour
{
    private string path => Application.persistentDataPath + "/settings.json";
    
    public void write_data(SettingsData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonData);
    }

    public SettingsData read_data()
    {
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<SettingsData>(jsonData);
        }
        else
        {
            write_data(SettingsData.Default);
            return SettingsData.Default;    
        }
    }
}

[System.Serializable]
public struct SettingsData
{
    public int level;
    public int bestScore;
    public bool showGhost;
    public SoundSettings sounds;
    public KeyBindings keys;
    
    public SettingsData(int level, int bestScore, bool showGhost, SoundSettings sounds, KeyBindings keys)
    {
        this.level = level;
        this.bestScore = bestScore;
        this.showGhost = showGhost;
        this.sounds = sounds;
        this.keys = keys;
    }

    public static SettingsData Default => new SettingsData(1, 0, true, SoundSettings.Default, KeyBindings.Default);
}

[System.Serializable]
public struct SoundSettings
{
    public float musicVolume;
    public float sfxVolume;
    public int musicType;

    public SoundSettings(float musicVolume, float sfxVolume, int musicType)
    {
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.musicType = musicType;
    }

    public static SoundSettings Default => new SoundSettings(1, 1, 0);
}

[System.Serializable]
public struct KeyBindings
{
    public KeyCode moveRight;
    public KeyCode moveLeft;
    public KeyCode rotateRight;
    public KeyCode rotateLeft;
    public KeyCode softDrop;
    public KeyCode instantDrop;
    public KeyCode hold;

    public KeyBindings(KeyCode moveRight, KeyCode moveLeft, KeyCode rotateRight, KeyCode rotateLeft, KeyCode softDrop, KeyCode instantDrop, KeyCode hold)
    {
        this.moveRight = moveRight;
        this.moveLeft = moveLeft;
        this.rotateRight = rotateRight;
        this.rotateLeft = rotateLeft;
        this.softDrop = softDrop;
        this.instantDrop = instantDrop;
        this.hold = hold;
    }

    public static KeyBindings Default => new KeyBindings(KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.Z, KeyCode.DownArrow, KeyCode.Space, KeyCode.C);

    public KeyCode this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return moveRight;
                case 1:
                    return moveLeft;
                case 2:
                    return rotateRight;
                case 3:
                    return rotateLeft;
                case 4:
                    return softDrop;
                case 5:
                    return instantDrop;
                case 6:
                    return hold;
                default:
                    return KeyCode.None;
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    moveRight = value;
                    break;
                case 1:
                    moveLeft = value;
                    break;
                case 2:
                    rotateRight = value;
                    break;
                case 3:
                    rotateLeft = value;
                    break;
                case 4:
                    softDrop = value;
                    break;
                case 5:
                    instantDrop = value;
                    break;
                case 6:
                    hold = value;
                    break;
            }
        }
    }
}
