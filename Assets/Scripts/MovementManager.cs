using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementManager : MonoBehaviour
{
    bool isLocked = false;
    public bool isGhost;
    bool isFoundPos;
    public bool canRotate;
    bool isHardDropping = false;
    public float dropRate;
    public float dropSpeedRate = 1;
    public float moveSpeed = 15;
    bool toRotate = false;
    int direction = 0;
    float time = 0;
    float Htime = 0;
    int RotateAngle = 0;
    int Point = 0;
    float acceleration = 0.5f;
    AudioSystem audioSystem;
    GameController gc;
    KeyBinder KeyBinder;


    void Start()
    {
        audioSystem = GameObject.FindWithTag("AuSys").GetComponent<AudioSystem>();
        gc = GameObject.FindWithTag("Background").GetComponent<GameController>();
        KeyBinder = GameObject.FindWithTag("Event System").GetComponent<KeyBinder>();
        UpdateSpeed();
        dropRate = dropSpeedRate;
    }
    void Update()
    {
        var sc = GameObject.FindWithTag("Game Menus").GetComponent<SceneController>();
        if (!isLocked && !gc.isPaused && !gc.isGameOver && !isGhost)
        {
            Drop();
            if (!isHardDropping)
            {
                RotateRight();
                RotateLeft();
                Rotate();
                MoveHorizontal();   
            }
            if (wentLeft())
            {
                goRight();
            }
            if (wentRight())
            {
                goLeft();
            }
            UpdateSpeed();
        }
        if (sc.showGhost && isGhost)
        {
            UpdateGhostPiece();
        }
    }


    void Drop()
    {
        var bg = GameObject.FindWithTag("Background").GetComponent<TetrominoSpawner>();
        var sc = GameObject.FindWithTag("Game Menus").GetComponent<SceneController>();
        time += Time.deltaTime;
        if(time >= 1/dropRate)
        {
            time = 0;
            if (canDrop())
            {
                transform.position += new Vector3(0, -1, 0);
                if (!isGhost)
                {
                    gc.Score += Point * gc.Level;
                }
            }
            else
            {
                if (!isGhost)
                {
                    if (gc.sfxOn)
                    {
                        audioSystem.playSound(GameObject.FindWithTag("LockedSound"));
                    }
                    isLocked = true;
                    gc.isHolded = false;
                    bg.ChoosenTetros[0] = bg.ChoosenTetros[2];
                    bg.ChoosenTetros[2] = bg.ChoosenTetros[3];
                    bg.ChoosenTetros[3] = bg.ChoosenTetros[4];
                    bg.ChoosenTetros[4] = bg.Randomize();
                    bg.Spawn();
                    bg.updateNext();
                    if (sc.showGhost)
                    {
                        Destroy(GameObject.FindWithTag("Ghost Tetromino"));
                        bg.Spawn_Ghost();
                    }
                    StoreCoordinates();
                    gc.removeRows();
                }
                else
                {
                    isFoundPos = true;
                }
            }
        }
        if (!isGhost)
        {
            if (Input.GetKey(KeyBinder.bindings[4]) && !isHardDropping)
            {
                dropRate = 15;
                Point = 1;
            }

            if (Input.GetKeyUp(KeyBinder.bindings[4]) && !isHardDropping)
            {
                dropRate = dropSpeedRate;
                Point = 0;
            }
            if (Input.GetKeyDown(KeyBinder.bindings[5]))
            {
                dropRate = 1000;
                Point = 2;
                isHardDropping = true;
            }
        }
    }


    bool canDrop()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int pos_y = Mathf.RoundToInt(transform.GetChild(i).position.y);
            int pos_x = Mathf.RoundToInt(transform.GetChild(i).position.x);
            if (pos_y <= GameObject.FindWithTag("BottomBorder").transform.position.y+1)
            {
                return false;
            }
            else
            {
                for (int j = 0; j < gc.Tiles.Count; j++)
                {
                    if (pos_y - 1 == Mathf.RoundToInt(gc.Tiles[j].transform.position.y) && pos_x == Mathf.RoundToInt(gc.Tiles[j].transform.position.x))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    void RotateRight()
    {
        if (Input.GetKeyDown(KeyBinder.bindings[2]))
        {
            RotateAngle = -90;
            toRotate = true;
        }
    }


    void RotateLeft()
    {
        if (Input.GetKeyDown(KeyBinder.bindings[3]))
        {
            RotateAngle = 90;
            toRotate = true;
        }
    }

    void Rotate()
    {
        var bd = GameObject.FindWithTag("BottomBorder");
        bool rotateSuccess = true;
        if (RotateAngle != 0 && canRotate)
        {
            transform.Rotate(0,0,RotateAngle);
            for (int i = 0; i < transform.childCount; i++)
            {
                int pos_y = Mathf.RoundToInt(transform.GetChild(i).position.y);
                int pos_x = Mathf.RoundToInt(transform.GetChild(i).position.x);
                if (pos_y <= bd.transform.position.y)
                {
                    transform.Rotate(0, 0, RotateAngle * -1);
                    rotateSuccess = false;
                    break;
                }
                for (int j = 0; j < gc.Tiles.Count; j++)
                {
                    if (pos_y  == Mathf.RoundToInt(gc.Tiles[j].transform.position.y) && pos_x == Mathf.RoundToInt(gc.Tiles[j].transform.position.x))
                    {
                        transform.Rotate(0, 0, RotateAngle * -1);
                        rotateSuccess = false;
                    }
                }
            }
        }
        RotateAngle = 0;
        if (gc.sfxOn && toRotate && rotateSuccess)
        {
            toRotate = false;
            audioSystem.playSound(GameObject.FindWithTag("RotateSound"));
        }
    }


    bool wentLeft()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).position.x < GameObject.FindWithTag("LeftBorder").transform.position.x+1)
            {
                return true;
            }
        }
        return false;
    }

    bool wentRight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).position.x > GameObject.FindWithTag("RightBorder").transform.position.x-1)
            {
                return true;
            }
        }
        return false;
    }

    void goLeft()
    {
        transform.position += new Vector3(-1, 0, 0);
    }

    void goRight()
    {
        transform.position += new Vector3(1, 0, 0);
    }

    void moveLeft()
    {
        if (Input.GetKey(KeyBinder.bindings[1]))
        {
            direction = -1;
        }

        if (Input.GetKeyUp(KeyBinder.bindings[1]))
        {
            direction = 0;
            Htime = 0;
        }
    }

    void MoveRight()
    {
        if (Input.GetKey(KeyBinder.bindings[0]))
        {
            direction = 1;
        }

        if (Input.GetKeyUp(KeyBinder.bindings[0]))
        {
            direction = 0;
            Htime = 0;
        }
    }

    void MoveHorizontal()
    {
        moveLeft();
        MoveRight();
        Htime += Time.deltaTime;
        if(Htime >= 1/moveSpeed)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int pos_y = Mathf.RoundToInt(transform.GetChild(i).position.y);
                int pos_x = Mathf.RoundToInt(transform.GetChild(i).position.x);
                for (int j = 0; j < gc.Tiles.Count; j++)
                {
                    if (pos_y  == Mathf.RoundToInt(gc.Tiles[j].transform.position.y) && pos_x + direction == Mathf.RoundToInt(gc.Tiles[j].transform.position.x))
                    {
                        transform.position += new Vector3(direction * -1, 0, 0);
                    }
                }
            }
            Htime = 0;
            transform.position += new Vector3(direction, 0, 0);
        }
    }

    void StoreCoordinates()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            int RowNumber = Mathf.RoundToInt(transform.GetChild(0).position.y - (GameObject.FindWithTag("BottomBorder").transform.position.y + 1));
            if (RowNumber > 13)
            {
                gc.Tiles.Add(transform.GetChild(0).gameObject);
                transform.GetChild(0).parent = null; 
                gc.isGameOver = true;
            }
            else
            {
                gc.Rows[RowNumber] += 1;
                gc.Tiles.Add(transform.GetChild(0).gameObject);
                transform.GetChild(0).parent = null;   
            }
        }
        if (gc.isGameOver)
        {
            GameObject.FindWithTag("Game Menus").GetComponent<SceneController>().enter_GameOverScreen();
        }
        Destroy(transform.gameObject);
    }

    void UpdateSpeed()
    {
        dropSpeedRate = 1 + ((gc.Level - 1) * acceleration);
    }

    void UpdateGhostPiece()
    {
        isFoundPos = false;
        GameObject tetromino = GameObject.FindWithTag("Tetromino");
        transform.position = new Vector3(tetromino.transform.position.x, tetromino.transform.position.y, 0);
        Vector3 tetroRotation = tetromino.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, 0, tetroRotation.z);
        while (true)
        {
            dropRate = 1000;
            Drop();
            if (isFoundPos)
            {
                break;
            }
        }
    }
}
