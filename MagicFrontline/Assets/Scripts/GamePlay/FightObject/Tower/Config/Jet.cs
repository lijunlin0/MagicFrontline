using System;
using UnityEngine;

public class Jet : Tower
{
    
    public static Jet Create(int level,Vector3Int position)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/Jet");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.position=position;
        Jet jet=towerObject.AddComponent<Jet>();
        var property=TowerUtility.GetBasePropertySheet("Jet",level);
        jet.Init(level,position,property);
        return jet;
    }


    public override void OnUpdate()
    {
        
    } 
}
