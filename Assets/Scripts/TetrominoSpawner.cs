using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    System.Random rnd = new System.Random();
    public GameObject[] Tetromino;
    public GameObject[] GhostTetromino;
    public GameObject SceneControl;
    [SerializeField] GameObject[] HoldedTetros;
    [SerializeField] GameObject[] FirstNextTetros;
    [SerializeField] GameObject[] SecondNextTetros;
    [SerializeField] GameObject[] ThirdNextTetros;


    public int dropHeight = 16;
    public int[] ChoosenTetros;


    void Start()
    {
        ChoosenTetros[0] = Randomize();
        ChoosenTetros[2] = Randomize();
        ChoosenTetros[3] = Randomize();
        ChoosenTetros[4] = Randomize();
        Spawn();
        updateNext();
        if (SceneControl.GetComponent<SceneController>().showGhost)
        {
            Spawn_Ghost();
        }
    }

    public void Spawn()
    {
        float average_x = (GameObject.FindWithTag("LeftBorder").transform.position.x + GameObject.FindWithTag("RightBorder").transform.position.x) / 2;
        Instantiate(Tetromino[ChoosenTetros[0]], new Vector3(average_x + 0.5f, GameObject.FindWithTag("BottomBorder").transform.position.y+dropHeight, 0), Quaternion.identity);
    }

    public void updateHold()
    {
        for (int i = 0; i < HoldedTetros.Length; i++)
        {
            if (i == ChoosenTetros[1])
            {
                HoldedTetros[i].SetActive(true);
            }
            else
            {
                HoldedTetros[i].SetActive(false);
            }
        }
    }

    public void updateNext()
    {
        for (int i = 0; i < FirstNextTetros.Length; i++)
        {
            if (i == ChoosenTetros[2])
            {
                FirstNextTetros[i].SetActive(true);
            }
            else
            {
                FirstNextTetros[i].SetActive(false);
            }
        }
        for (int i = 0; i < SecondNextTetros.Length; i++)
        {
            if (i == ChoosenTetros[3])
            {
                SecondNextTetros[i].SetActive(true);
            }
            else
            {
                SecondNextTetros[i].SetActive(false);
            }
        }
        for (int i = 0; i < ThirdNextTetros.Length; i++)
        {
            if (i == ChoosenTetros[4])
            {
                ThirdNextTetros[i].SetActive(true);
            }
            else
            {
                ThirdNextTetros[i].SetActive(false);
            }
        }
    }

    public void Spawn_Ghost()
    {
        float average_x = (GameObject.FindWithTag("LeftBorder").transform.position.x + GameObject.FindWithTag("RightBorder").transform.position.x) / 2;
        Instantiate(GhostTetromino[ChoosenTetros[0]], new Vector3(average_x + 0.5f, GameObject.FindWithTag("BottomBorder").transform.position.y+dropHeight, 0), Quaternion.identity);
    }

    public int Randomize()
    {
        while (true)
        {
            int random = rnd.Next(Tetromino.Length);
            int index = System.Array.IndexOf(ChoosenTetros, random);
            if (index == -1 || index == 1)
            {
                return random;
            }
        }
    }
}
