using System;
using LitJson;
using UnityEngine;

public class TowerUtility
{
    private static JsonData Config;
    public static void Init()
    {
        TextAsset configText=Resources.Load<TextAsset>("Config/TowerConfig");
        Config=JsonMapper.ToObject(configText.text);
    }

    public static Tuple<int,float,int> GetBasePropertySheet(string towerName,int level)
    {
        JsonData towerData=Config[towerName];
        
        Debug.Log("等级:"+level);
        int attack=(int)towerData["Attack"][level-1];
        float attackInterval=(float)(double)towerData["AttackInterval"][level-1];
        int shootRange=(int)towerData["ShootRange"][level-1];
        Debug.Log("等级:"+level+",伤害:"+attack+",攻击间隔:"+attackInterval+",攻击范围:"+shootRange);
        Tuple<int,float,int> property=new Tuple<int, float,int>(attack,attackInterval,shootRange);
        return property;
    }

}