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
        var lastData = ds.read_data();
        lastData.sounds.musicVolume = Sliders[1].value;
        lastData.sounds.sfxVolume = Sliders[0].value;
        ds.write_data(lastData);
    }

    void readSounds()
    {
        var soundSettings = ds.read_data().sounds;

        Sliders[0].value = soundSettings.sfxVolume;
        Sliders[1].value = soundSettings.musicVolume;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            foreach (var music in musics)
            {
                music.GetComponent<AudioSource>().volume = soundSettings.musicVolume;
            }
        }    
        foreach (var sfx in sfxs)
        {
            sfx.volume = soundSettings.sfxVolume;
        }
    }

    void readType()
    {
        int musicType = ds.read_data().sounds.musicType;
        
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
