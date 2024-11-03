using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class LevelUtility
{
    private static JsonData Config;
    private static int mLevelCount=0;
    public static void Init()
    {
        TextAsset configText=Resources.Load<TextAsset>("Config/LevelConfig");
        Config=JsonMapper.ToObject(configText.text);
        for(int i =1;i<=Config.Keys.Count;i++)
        {
            mLevelCount++;
            PlayerPrefs.SetInt("Level"+i.ToString(),0);
        }
        Debug.Log("共有"+mLevelCount+"关");
        PlayerPrefs.Save();
    }

    public static List<Vector3Int> GetTurningPoints(int level)
    {
        string levelName="Level"+level.ToString();
        JsonData jsonData=Config[levelName];
        List<Vector3Int> points=new List<Vector3Int>();
        int health=(int)jsonData[0]["Health"];
        int coins=(int)jsonData[1]["Coins"];
        FightModel.GetCurrent().SetHealth(health);
        FightModel.GetCurrent().SetCoins(coins);
        FightModel.GetCurrent().SetLevel(level);
        for (int i = 2; i < jsonData.Count; i++)
        {
            // 获取每个坐标的 x, y, z 值
            int x = (int)jsonData[i]["x"];
            int y = (int)jsonData[i]["y"];
            points.Add(new Vector3Int(x, y, 0));
        }
        
        return points;
    }

    public static void SetLevelPass(int level)
    {
        PlayerPrefs.SetInt("Level"+level.ToString(),1);
        PlayerPrefs.Save();
    }
    public static bool GetLevelPass(int level)
    {
        return PlayerPrefs.GetInt("Level"+level.ToString())==1;
    }

}