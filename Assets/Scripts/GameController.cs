using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public List<GameObject> Tiles = new List<GameObject>();
    public List<int> Rows = new List<int>();
    public List<int> CompletedRows = new List<int>();
    [SerializeField] TextMeshProUGUI[] GameBoard;
    [SerializeField] GameObject AudioObject;
    [SerializeField] GameObject[] HoldSounds;
    [SerializeField] GameObject MoveSound;
    [SerializeField] GameObject LineClearSound;
    [SerializeField] KeyBinder keyBinder;

    public bool isHolded = false;
    public int Score = 0;
    public int Start_Level;
    public int Level;
    public int LineCount = 0;
    public bool isPaused = false;
    public bool isGameOver = false;
    public bool musicOn = true;
    public bool sfxOn = true;
    int last_x;


    

    void Start()
    {
        var ds = GameObject.FindWithTag("Background").GetComponent<DataStorer>();
        Start_Level = ds.read_data().level;
        Level = Start_Level;
        var tetro = GameObject.FindWithTag("Tetromino");
        last_x = Mathf.RoundToInt(tetro.transform.position.x);
    }

    void Update()
    {
        if (!isPaused && !isGameOver)
        {
            HoldTetromino();
            UpdateScore();
            UpdateLevel();
            UpdateLineCount();
            if (sfxOn && moveSuccess())
            {
                AudioObject.GetComponent<AudioSystem>().playSound(MoveSound);
            }
        }
    }

    public void removeRows()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            var num = i + Mathf.RoundToInt((GameObject.FindWithTag("BottomBorder").transform.position.y + 1));
            if (Rows[i] == 10)
            {
                CompletedRows.Add(num);
            }
        }

        int ind = 0;
        while (true)
        {
            try
            {
                int pos_y = Mathf.RoundToInt(Tiles[ind].transform.position.y);
                if (CompletedRows.Contains(pos_y))
                {
                    Destroy(Tiles[ind].transform.gameObject);
                    Tiles.RemoveAt(ind);
                    ind--;
                }
                ind++;
            }
            catch (System.Exception)
            {
                break;
            }
        }

        if (CompletedRows.Count == 1)
        {
            Score += 100 * Level;
        }
        else if (CompletedRows.Count == 2)
        {
            Score += 300 * Level;
        }
        else if (CompletedRows.Count == 3)
        {
            Score += 500 * Level;
        }
        else if (CompletedRows.Count == 4)
        {
            Score += 800 * Level;
        }
        LineCount += CompletedRows.Count;

        if (sfxOn && CompletedRows.Count != 0)
        {
            AudioObject.GetComponent<AudioSystem>().playSound(LineClearSound);
        }

        int lastIndex = CompletedRows.Count - 1;
        while (lastIndex >= 0)
        {
            Rows.RemoveAt(CompletedRows[lastIndex] - Mathf.RoundToInt((GameObject.FindWithTag("BottomBorder").transform.position.y + 1)));
            Rows.Add(0);
            foreach (var tile in Tiles)
            {
                int pos_y = Mathf.RoundToInt(tile.transform.position.y);
                if (pos_y > CompletedRows[lastIndex])
                {
                    tile.transform.position += new Vector3(0, -1, 0);
                }
            }
            CompletedRows.RemoveAt(lastIndex);
            lastIndex--;
        }
    }

    public void HoldTetromino()
    {
        var ts = GameObject.FindWithTag("Background").GetComponent<TetrominoSpawner>();
        var sc = GameObject.FindWithTag("Game Menus").GetComponent<SceneController>();
        if (Input.GetKeyDown(keyBinder.bindings[6]))
        {
            if (isHolded)
            {
                if (sfxOn)
                {
                    AudioObject.GetComponent<AudioSystem>().playSound(HoldSounds[1]);
                }
            }
            else
            {
                if (ts.ChoosenTetros[1] == -1)
                {
                    ts.ChoosenTetros[1] = ts.ChoosenTetros[0];
                    ts.ChoosenTetros[0] = ts.ChoosenTetros[2];
                    ts.ChoosenTetros[2] = ts.ChoosenTetros[3];
                    ts.ChoosenTetros[3] = ts.ChoosenTetros[4];
                    ts.ChoosenTetros[4] = ts.Randomize();
                }
                else
                {
                    int i0 = ts.ChoosenTetros[0];
                    int i1 = ts.ChoosenTetros[1];
                    ts.ChoosenTetros[0] = i1;
                    ts.ChoosenTetros[1] = i0;
                }
                Destroy(GameObject.FindWithTag("Tetromino"));
                ts.Spawn();
                ts.updateNext();
                ts.updateHold();
                if (sc.showGhost)
                {
                    Destroy(GameObject.FindWithTag("Ghost Tetromino"));
                    ts.Spawn_Ghost();
                }
                isHolded = true;
                if (sfxOn)
                {
                    AudioObject.GetComponent<AudioSystem>().playSound(HoldSounds[0]);
                }
            }
        }
    }

    void UpdateScore()
    {
        GameBoard[0].text = Score.ToString();
    }

    void UpdateLevel()
    {
        Level = Start_Level + (LineCount / 10);
        GameBoard[1].text = Level.ToString();
    }

    void UpdateLineCount()
    {
        GameBoard[2].text = LineCount.ToString();
    }


    bool moveSuccess()
    {
        var rd = GameObject.FindWithTag("RightBorder");
        var ld = GameObject.FindWithTag("LeftBorder");
        var tetro = GameObject.FindWithTag("Tetromino");
        if (Mathf.RoundToInt(tetro.transform.position.x) != last_x)
        {
            last_x = Mathf.RoundToInt(tetro.transform.position.x);
            for (int i = 0; i < tetro.transform.childCount; i++)
            {
                int pos_y = Mathf.RoundToInt(tetro.transform.GetChild(i).position.y);
                int pos_x = Mathf.RoundToInt(tetro.transform.GetChild(i).position.x);
                if (pos_x <= ld.transform.position.x || pos_x >= rd.transform.position.x)
                {
                    return false;
                }
                for (int j = 0; j < Tiles.Count; j++)
                {
                    if (pos_y  == Mathf.RoundToInt(Tiles[j].transform.position.y) && pos_x == Mathf.RoundToInt(Tiles[j].transform.position.x))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
