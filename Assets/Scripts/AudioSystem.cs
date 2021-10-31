using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] GameObject[] musics;
    [SerializeField] AudioSource[] sfxs;

    [SerializeField] Slider[] Sliders;
    [SerializeField] DataStorer ds;
    [SerializeField] TextMeshProUGUI MusicTypeText;
    [SerializeField] GameObject[] TypeButtons;
    public int currentMusic;
    public int currentPlay;

    void Start()
    {
        readSounds();
        readType();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playMusic();
        }
    }
    
    public void playMusic()
    {
        musics[currentMusic].GetComponent<AudioSource>().Play();
        currentPlay = currentMusic;
    }

    public void stopMusic()
    {
        musics[currentMusic].GetComponent<AudioSource>().Stop();
    }

    public void pauseMusic()
    {
        musics[currentMusic].GetComponent<AudioSource>().Pause();
    }

    public void unpauseMusic()
    {
        musics[currentMusic].GetComponent<AudioSource>().UnPause();
    }

    public void playSound(GameObject sound)
    {
        sound.GetComponent<AudioSource>().Play();
    }

    public void updateMusicVolume()
    {
        foreach (var music in musics)
        {
            music.GetComponent<AudioSource>().volume = Sliders[1].value;
        }
        writeSounds();
    }

    public void updateSFXVolume()
    {
        foreach (var sound in sfxs)
        {
            sound.volume = Sliders[0].value;
        }
        writeSounds();
    }

    void writeSounds()
    {
        var datas = ds.read_data();
        string newData = "";
        for (int i = 0; i < datas.Length; i++)
        {
            if (i == 3)
            {
                newData += "MusicVol:" + Sliders[1].value + "\n";
            }
            else if (i == 4)
            {
                newData += "SFXVol:" + Sliders[0].value + "\n";
            }
            else
            {
                newData += datas[i] + "\n";
            }
        }
        ds.write_data(newData.Substring(0, newData.Length - 1));
    }

    void readSounds()
    {
        var datas = ds.read_data();
        float SFXVol = float.Parse(datas[4].Substring(7, datas[4].Length - 7));
        float MusicVol = float.Parse(datas[3].Substring(9, datas[3].Length - 9));
        Sliders[0].value = SFXVol;
        Sliders[1].value = MusicVol;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            foreach (var music in musics)
            {
                music.GetComponent<AudioSource>().volume = MusicVol;
            }
        }    
        foreach (var sfx in sfxs)
        {
            sfx.volume = SFXVol;
        }
    }

    void readType()
    {
        string data = ds.read_data()[5];
        int musicType = System.Convert.ToInt32(data.Substring(10, data.Length - 10));
        currentMusic = musicType;
        MusicTypeText.text = "Type " + (musicType + 1).ToString();
        if (musicType == 0)
        {
            TypeButtons[0].SetActive(false);
            TypeButtons[1].SetActive(true);
        }
        else if (musicType == 1)
        {
            TypeButtons[0].SetActive(true);
            TypeButtons[1].SetActive(true);
        }
        else if (musicType == 2)
        {
            TypeButtons[0].SetActive(true);
            TypeButtons[1].SetActive(false);
        }
    }
    
}
