using System;
using LitJson;
using UnityEngine;

public class EnemyUtility
{
    private static JsonData Config;
    public static void Init()
    {
        TextAsset configText=Resources.Load<TextAsset>("Config/EnemyConfig");
        Config=JsonMapper.ToObject(configText.text);
    }

    public static Property GetBasePropertySheet(string enemyName,int level)
    {
        JsonData enemyData=Config[enemyName];
        Property property=new Property();
        
        int health=(int)enemyData["Health"][0];
        int healthAdd=(int)enemyData["Health"][1];
        int moveSpeed=(int)enemyData["MoveSpeed"];

        property.Init(health,healthAdd,moveSpeed);
        return property;
    }

}