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
        string strKey = key.ToString();
        var datas = dataStorer.read_data();
        string newData = "";
        for (int i = 0; i < datas.Length; i++)
        {
            if (i == index + 6)
            {
                newData += datas[i].Split(char.Parse(":"))[0] + ":" + strKey + "\n";
            }
            else
            {
                newData += datas[i] + "\n";
            }
        }
        dataStorer.write_data(newData.Substring(0, newData.Length - 1));
        if (strKey.Length > 5 && strKey.Substring(0, 5) == "Alpha")
        {
            strKey = strKey.Substring(5, 1);
        }
        KeyBindTexts[index].text = strKey;
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
        for (int i = 0; i < data.Length; i++)
        {
            if (i >= 6 && i <= 12)
            {
                string strKey = data[i].Split(char.Parse(":"))[1];
                bindings[i-6] = ToKeyCode(strKey);
                if (strKey.Length > 5 && strKey.Substring(0, 5) == "Alpha")
                {
                    strKey = strKey.Substring(5, 1);
                }
                KeyBindTexts[i-6].text = strKey;
            }
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
