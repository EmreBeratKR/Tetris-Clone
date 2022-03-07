using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KeyBinder : MonoBehaviour
{

    [SerializeField] DataStorer dataStorer;
    public bool isListening = false;
    public int bindIndex;
    public string lastBind;
    [SerializeField] KeyCode[] validKeys;
    [SerializeField] KeyCode[] defaultBinding;
    public KeyCode[] bindings;
    public TextMeshProUGUI[] KeyBindTexts;
    public GameObject BindJammer;
    [SerializeField] GameObject BindingError;

    void Start()
    {
        readKeyCode();
        updateBindError();
    }

    void Update()
    {
        if (isListening)
        {
            listenKey(bindIndex);
        }
    }

    void listenKey(int index)
    {
        foreach(KeyCode key in validKeys)
        {
            if(Input.GetKeyDown(key))
            {
                changeBinding(index, key);
                for (int i = 0; i < bindings.Length; i++)
                {
                    if (i != index && bindings[i] == key)
                    {
                        changeBinding(i, KeyCode.None);
                    }
                }
            }
        }
    }

    public void bindKey(int index)
    {
        lastBind = KeyBindTexts[index].text;
        isListening = true;
        BindJammer.SetActive(true);
        KeyBindTexts[index].text = "Binding..."; 
        bindIndex = index;
    }

    void changeBinding(int index, KeyCode key)
    {
        var lastData = dataStorer.read_data();

        lastData.keys[index] = key;

        dataStorer.write_data(lastData);

        KeyBindTexts[index].text = key.ToString();
        BindJammer.SetActive(false);
        isListening = false;
        bindings[index] = key;
        updateBindError();
    }

    bool keyMissing()
    {
        foreach (var kcode in bindings)
        {
            if (kcode == KeyCode.None)
            {
                return true;
            }
        }
        return false;
    }

    void updateBindError()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (keyMissing())
            {
                BindingError.SetActive(true);
            }
            else
            {
                BindingError.SetActive(false);
            }
        }
    }

    public KeyCode ToKeyCode(string key)
    {
        foreach(KeyCode kcode in validKeys)
        {
            if (kcode.ToString() == key)
            {
                return kcode;
            }
        }
        return KeyCode.None; //never works
    }

    void readKeyCode()
    {
        var data = dataStorer.read_data();

        for (int i = 0; i < bindings.Length; i++)
        {
            bindings[i] = data.keys[i];
            KeyBindTexts[i].text = data.keys[i].ToString();
        }
    }

    public void resetBindings()
    {
        for (int b = 0; b < bindings.Length; b++)
        {
            changeBinding(b, defaultBinding[b]);
        }
    }
}
