using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class LevelEnemyCreateUtility
{
    private static JsonData Config;
    public static void Init()
    {
        TextAsset configText=Resources.Load<TextAsset>("Config/LevelEnemyCreateConfig");
        Config=JsonMapper.ToObject(configText.text);
    }

    public static List<Tuple<EnemyId, int>> GetBaseEnemyCreateList(int level)
    {
        Debug.Log(level);
        JsonData jsonData = Config["Level" + level.ToString()];

        List<Tuple<EnemyId, int>> enemyList = new List<Tuple<EnemyId, int>>();

        for(int i=0;i<jsonData.Count;i++)
        {
                // 访问每个敌人字典
            // 获取敌人的第一个键值对
            string enemyName = (string)jsonData[i][0];
            int enemyCount = (int)jsonData[i][1];

            // 转换为 EnemyId
            EnemyId enemyId = (EnemyId)Enum.Parse(typeof(EnemyId), enemyName);
            enemyList.Add(new Tuple<EnemyId, int>(enemyId, enemyCount));
        }

        return enemyList;
    }


}