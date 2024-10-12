using System;
using UnityEngine;

public class LightwaveStone : Tower
{
    
    public static LightwaveStone Create(int level,Vector3Int position)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/LightwaveStone");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.position=position;
        LightwaveStone lightwaveStone=towerObject.AddComponent<LightwaveStone>();
        var property=TowerUtility.GetBasePropertySheet("LightwaveStone",level);
        lightwaveStone.Init(level,position,property);
        return lightwaveStone;
    }
    public override void OnUpdate()
    {
        
    } 
}
