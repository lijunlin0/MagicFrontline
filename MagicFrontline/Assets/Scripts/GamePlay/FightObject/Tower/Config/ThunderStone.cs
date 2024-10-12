using System;
using UnityEngine;

public class ThunderStone : Tower
{
    
    public static ThunderStone Create(int level,Vector3Int position)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/ThunderStone");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.position=position;
        ThunderStone thunderStone=towerObject.AddComponent<ThunderStone>();
        var property=TowerUtility.GetBasePropertySheet("ThunderStone",level);
        thunderStone.Init(level,position,property);
        return thunderStone;
    }
    public override void OnUpdate()
    {
        
    } 
}
