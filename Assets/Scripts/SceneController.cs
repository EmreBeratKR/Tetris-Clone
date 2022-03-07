using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class SceneController : MonoBehaviour
{

    public GameObject[] PauseMenu;
    public GameObject[] GameOverScreen;
    public GameObject[] SettingsMenu;
    public GameObject[] HowToMenu;
    public GameObject KeyBindingMenu;
    public Slider MusicSlider;
    public GameObject GhostToggle;
    public GameObject DataStorer;
    [SerializeField] GameObject AudioSystem;
    [SerializeField] GameObject[] TypeButtons;
    [SerializeField] GameObject GameOverSound;

    Toggle gt;
    AudioSystem msc;

    int Level = 1;
    bool inSettings;
    bool inHowTo;
    bool inKeyBindings;
    public bool showGhost;
    float lastMusicVolume;

    [SerializeField] TextMeshProUGUI Level_Object;
    [SerializeField] TextMeshProUGUI[] Scores;
    [SerializeField] TextMeshProUGUI MusicTypeText;
    [SerializeField] TextMeshProUGUI[] KeyBindTexts;
    [SerializeField] KeyBinder KeyBinder;

    void Start()
    {
        if (AudioSystem != null)
        {
            msc = AudioSystem.GetComponent<AudioSystem>();
        }
        DataStorer ds = DataStorer.GetComponent<DataStorer>();
        gt = GhostToggle.GetComponent<Toggle>();
        
        var datas = ds.read_data();

        showGhost = datas.showGhost;
        gt.isOn = datas.showGhost;

        /* string data = datas[2].Substring(10, datas[2].Length - 10);
        if (data == "True")
        {
            showGhost = true;
            gt.isOn = true;
        }
        else if (data == "False")
        {
            showGhost = false;
            gt.isOn = false;
        } */
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            var gc = GameObject.FindWithTag("Background").GetComponent<GameController>();
            if (Input.GetKeyDown(KeyCode.Escape) && !gc.isGameOver)
            {
                if (inKeyBindings)
                {
                    if (!KeyBinder.isListening)
                    {
                        close_keybinding();
                    }
                    else
                    {
                        KeyBinder.KeyBindTexts[KeyBinder.bindIndex].text = KeyBinder.lastBind;
                        KeyBinder.BindJammer.SetActive(false);
                        KeyBinder.isListening = false;
                    }
                }
                else if (inSettings)
                {
                    close_settings();
                }
                else if (inHowTo)
                {
                    close_howto();
                }
                else
                {
                    if (!gc.isPaused)
                    {
                        pause_game();
                    }
                    else if (gc.isPaused)                           
                    {
                    resume_game();
                    }
                }
            }
        }
        else //ilerde yeni scene eklenirse sıkıntı çıkarabilir!
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (inKeyBindings)
                {
                    if (!KeyBinder.isListening)
                    {
                        close_keybinding();
                    }
                    else
                    {
                        KeyBinder.KeyBindTexts[KeyBinder.bindIndex].text = KeyBinder.lastBind;
                        KeyBinder.BindJammer.SetActive(false);
                        KeyBinder.isListening = false;
                    }
                }
                else if (inSettings)
                {
                    close_settings();
                }
                else if (inHowTo)
                {
                    close_howto();
                }
            }
        }
    }
    public void load_game()
    {
        var ds = GameObject.FindWithTag("Main Menu").GetComponent<DataStorer>();
        var datas = ds.read_data();
        datas.level = Level;
        ds.write_data(datas);
        /* string newData = "";
        for (int i = 0; i < datas.Length; i++)
        {
            if (i == 0)
            {
                newData += "Level:" + Level.ToString() + "\n";
            }
            else
            {
                newData += datas[i] + "\n";
            }
        }
        ds.write_data(newData.Substring(0, newData.Length - 1)); */
        SceneManager.LoadScene(1);
    }

    public void restart_game()
    {
        SceneManager.LoadScene(1);
    }

    public void level_set()
    {
        if (Level == 1)
        {
            Level = 5;
            Level_Object.text = "Level: 5";
        }
        else if (Level == 5)
        {
            Level = 10;
            Level_Object.text = "Level: 10";
        }
        else if (Level == 10)
        {
            Level = 15;
            Level_Object.text = "Level: 15";
        }
        else if (Level == 15)
        {
            Level = 20;
            Level_Object.text = "Level: 20";
        }
        else if (Level == 20)
        {
            Level = 25;
            Level_Object.text = "Level: 25";
        }
        else if (Level == 25)
        {
            Level = 1;
            Level_Object.text = "Level: 1";
        }
    }

    public void open_settings()
    {
        foreach (var obj in SettingsMenu)
        {
            obj.SetActive(true);
        }
        inSettings = true;
    }

    public void close_settings()
    {
        foreach (var obj in SettingsMenu)
        {
            obj.SetActive(false);
        }
        inSettings = false;
    }

    public void UpdateShowGhost()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (!gt.isOn)
        {
            showGhost = false;
            if (SceneIndex == 1)
            {
                Destroy(GameObject.FindWithTag("Ghost Tetromino"));
            }
        }
        else
        {
            showGhost = true;
            if (SceneIndex == 1)
            {
                var ts = GameObject.FindWithTag("Background").GetComponent<TetrominoSpawner>();
                ts.Spawn_Ghost();
            }
        }
        DataStorer ds = DataStorer.GetComponent<DataStorer>();
        var datas = ds.read_data();

        datas.showGhost = showGhost;
        ds.write_data(datas);


        /* string newData = "";
        for (int i = 0; i < datas.Length; i++)
        {
            if (i == 2)
            {
                newData += "ShowGhost:" + showGhost.ToString() + "\n";
            }
            else
            {
                newData += datas[i] + "\n";
            }
        }
        ds.write_data(newData.Substring(0, newData.Length - 1)); */
    }

    public void upperType()
    {
        DataStorer ds = DataStorer.GetComponent<DataStorer>();
        msc.currentMusic += 1;
        TypeButtons[0].SetActive(true);
        MusicTypeText.text = "Type " + (msc.currentMusic + 1).ToString();
        if (msc.currentMusic == 2)
        {
            TypeButtons[1].SetActive(false);
        }
        var datas = ds.read_data();

        datas.sounds.musicType = msc.currentMusic;
        ds.write_data(datas);

        /* string newData = "";
        for (int i = 0; i < datas.Length; i++)
        {
            if (i == 5)
            {
                newData += "MusicType:" + msc.currentMusic.ToString() + "\n";
            }
            else
            {
                newData += datas[i] + "\n";
            }
        }
        ds.write_data(newData.Substring(0, newData.Length - 1)); */
    }

    public void lowerType()
    {
        DataStorer ds = DataStorer.GetComponent<DataStorer>();
        msc.currentMusic -= 1;
        TypeButtons[1].SetActive(true);
        MusicTypeText.text = "Type " + (msc.currentMusic + 1).ToString();
        if (msc.currentMusic == 0)
        {
            TypeButtons[0].SetActive(false);
        }
        var datas = ds.read_data();

        datas.sounds.musicType = msc.currentMusic;
        ds.write_data(datas);

        /* string newData = "";
        for (int i = 0; i < datas.Length; i++)
        {
            if (i == 5)
            {
                newData += "MusicType:" + msc.currentMusic.ToString() + "\n";
            }
            else
            {
                newData += datas[i] + "\n";
            }
        }
        ds.write_data(newData.Substring(0, newData.Length - 1)); */
    }
    

    public void load_main()
    {
        SceneManager.LoadScene(0);
    }

    public void pause_game()
    {
        lastMusicVolume = MusicSlider.value;
        var gc = GameObject.FindWithTag("Background").GetComponent<GameController>();
        PauseMenu[0].SetActive(true);
        PauseMenu[1].SetActive(true);
        gc.isPaused = true;
        msc.pauseMusic();
    }

    public void resume_game()
    {
        var gc = GameObject.FindWithTag("Background").GetComponent<GameController>();
        PauseMenu[0].SetActive(false);  
        PauseMenu[1].SetActive(false); 
        gc.isPaused = false;
        if (msc.currentMusic != msc.currentPlay)
        {
            msc.stopMusic();
            msc.playMusic();
        }
        else
        {
            if (lastMusicVolume != 0)
            {
                msc.unpauseMusic();
            }
            else
            {
                msc.stopMusic();
                msc.playMusic();
            }   
        }
    }

    public void open_howto_pg1()
    {
        HowToMenu[0].SetActive(true);
        HowToMenu[1].SetActive(false); 
        HowToMenu[2].SetActive(false);
        if (HowToMenu.Length == 4)
        {
            HowToMenu[3].SetActive(true);
        }
        inHowTo = true;
    }

    public void open_howto_pg2()
    {
        HowToMenu[0].SetActive(false);
        HowToMenu[1].SetActive(true); 
        HowToMenu[2].SetActive(false); 
    }

    public void open_howto_pg3()
    {
        HowToMenu[0].SetActive(false);
        HowToMenu[1].SetActive(false); 
        HowToMenu[2].SetActive(true);
    }

    public void close_howto()
    {
        HowToMenu[0].SetActive(false);
        HowToMenu[1].SetActive(false);
        HowToMenu[2].SetActive(false);
        if (HowToMenu.Length == 4)
        {
            HowToMenu[3].SetActive(false);
        }
        inHowTo = false;
    }
    public void open_keybinding()
    {
        KeyBindingMenu.SetActive(true);
        inKeyBindings = true;
    }

    public void close_keybinding()
    {
        KeyBindingMenu.SetActive(false);
        inKeyBindings = false;
    }

    public void enter_GameOverScreen()
    {
        msc.stopMusic();
        msc.playSound(GameOverSound);
        var gc = GameObject.FindWithTag("Background").GetComponent<GameController>();
        Scores[0].text = "Your Score:\n" + gc.Score.ToString();
        var ds = GameObject.FindWithTag("Background").GetComponent<DataStorer>();
        //int last_best = System.Convert.ToInt32(ds.read_data()[1].Substring(5));
        var datas = ds.read_data();
        int last_best = datas.bestScore;
        bool is_new;
        if (gc.Score > last_best)
        {
            /* var datas = ds.read_data();
            string newData = "";
            for (int i = 0; i < datas.Length; i++)
            {
                if (i == 1)
                {
                    newData += "Best:" + gc.Score.ToString() + "\n";
                }
                else
                {
                    newData += datas[i] + "\n";
                }
            }
            ds.write_data(newData.Substring(0, newData.Length - 1)); */

            datas.bestScore = gc.Score;
            ds.write_data(datas);

            Scores[1].text = "Your Best:\n" + gc.Score.ToString();
            is_new = true;
        }
        else
        {
         Scores[1].text = "Your Best:\n" + last_best.ToString();
         is_new = false;
        }
        GameOverScreen[0].SetActive(true);
        GameOverScreen[1].SetActive(true);
        if (!is_new)
        {
            GameOverScreen[2].SetActive(false);
        }
    }

    public void exit_game()
    {
        Debug.Log("Oyundan Çıkıldı!");
        Application.Quit();
    }
}
