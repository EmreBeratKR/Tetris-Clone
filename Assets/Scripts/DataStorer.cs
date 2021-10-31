using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStorer : MonoBehaviour
{
    public void write_data(string data)
    {
        string path = "Assets/Data/data.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(data);
        writer.Close();
    }

    public string[] read_data()
    {
        string path = "Assets/Data/data.txt";
        StreamReader reader = new StreamReader(path);
        string[] data = reader.ReadToEnd().Split(char.Parse("\n"));
        reader.Close();
        return data;
    }
}
