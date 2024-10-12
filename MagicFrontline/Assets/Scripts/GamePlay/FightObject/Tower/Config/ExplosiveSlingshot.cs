using System;
using UnityEngine;

public class ExplosiveSlingshot : Tower
{
    
    public static ExplosiveSlingshot Create(int level,Vector3Int position)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/ExplosiveSlingshot");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.position=position;
        ExplosiveSlingshot explosiveSlingshot=towerObject.AddComponent<ExplosiveSlingshot>();
         var property=TowerUtility.GetBasePropertySheet("ExplosiveSlingshot",level);
        explosiveSlingshot.Init(level,position,property);
        return explosiveSlingshot;
    }
    
    public override void OnUpdate()
    {
        
    } 
}
