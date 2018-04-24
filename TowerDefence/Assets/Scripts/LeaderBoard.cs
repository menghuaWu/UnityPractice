using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LeaderBoard : MonoBehaviour {

    public static LeaderBoard _instance;

    private ScoreData data;

    private const int Places = 3;

    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;

            LoadData();
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        if (File.Exists(Application.dataPath + "/score.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/score.dat", FileMode.Open);

            data = (ScoreData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            InitData();
            SaveData();
        }

        throw new NotImplementedException();
    }

    private void InitData()
    {
        data = new ScoreData();
        data.score = new int[Places];
        data.id = new int[Places];

        for (int i=0; i < Places; i++)
        {
            data.score[i] = 0;
            data.id[i] = -1;
        }
        throw new NotImplementedException();
    }

    private void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/score.dat");

        bf.Serialize(file, data);
        file.Close();

        throw new NotImplementedException();
    }

    public void NewScore(int id, int score)
    {
        int place = Places - 1;
        while (place >= 0 && score > data.score[place])
        {
            place --;
        }

        place++;
        if (place >= Places)
        {
            return;
        }
        for (int i = Places-2; i>=place; i++)
        {
            data.score[i + 1] = data.score[i];
            data.id[i + 1] = data.id[i];
        }

        data.score[place] = score;
        data.id[place] = id;

        SaveData();
    }

    public int GetScore(int id)
    {
        return data.score[id];
    }

    public int GetID(int id)
    {
        return data.id[id];
    }

}
