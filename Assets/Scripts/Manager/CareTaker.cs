using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class CareTaker
{
    private Memento memento;

    public Memento GetMemento()
    {
        return memento;
    }

    public void SetMemento(Memento memento)
    {
        this.memento = memento;
    }

    public void SetMementoFromFile(string path)
    {
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            PlayerManager playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
            SetMemento(playerManager.createMemento());
        }
        else
        {
            Debug.Log("PlayerManager读取失败");
            StreamReader sr = new StreamReader(StringManager.playerManagerInitDataFilePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            PlayerManager playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
            SetMemento(playerManager.createMemento());
        }
    }
}
