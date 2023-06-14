using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsHandler : Singleton<PlayerPrefsHandler>
{
    public int gold;
    public List<int> collectedGems;

    void Start()
    {
        LoadPrefs();
        UIHandler.Instance.InitUI();
    }

    #region SaveAndLoad
    void LoadPrefs()
    {
        string jsonData = PlayerPrefs.GetString("GameData", "");

        if (string.IsNullOrEmpty(jsonData))
        {
            LoadDefaultValues();
            return;
        }

        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
        gold = gameData.gold;
        collectedGems = gameData.gemCounts;
    }

    void SavePrefs()
    {
        GameData gameData = new GameData(gold, collectedGems);
        string jsonData = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("GameData", jsonData);
    }

    void LoadDefaultValues()
    {
        collectedGems = new List<int>();
        for (int i = 0; i < GameManager.Instance.gems.Count; i++)
        {
            collectedGems.Add(0);
        }
    }

    #endregion

    public void IncreaseGold(int value)
    {
        gold += value;
        UIHandler.Instance.AnimateGoldText(gold);
    }

    public void IncreaseCollectedGems(int index)
    {
        if (index >= collectedGems.Count)
        {
            int diff = index - collectedGems.Count - 1;
            for (int i = 0; i < diff; i++)
            {
                collectedGems.Add(0);
            }
        }

        collectedGems[index]++;
    }

    public void OnApplicationQuit()
    {
        SavePrefs();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            SavePrefs();
    }
}

[Serializable]
public struct GameData
{
    public int gold;
    public List<int> gemCounts;

    public GameData(int _gold, List<int> _gemCounts)
    {
        gold = _gold;
        gemCounts = _gemCounts;
    }
}
