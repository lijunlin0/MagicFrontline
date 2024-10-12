using System;
using UnityEngine;

public class FrozenBow : Tower
{
    public static FrozenBow Create(int level,Vector3Int position)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/FrozenBow");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.position=position;
        FrozenBow frozenBow=towerObject.AddComponent<FrozenBow>();
        var property=TowerUtility.GetBasePropertySheet("FrozenBow",level);
        frozenBow.Init(level,position,property);
        return frozenBow;
    }
    public override void OnUpdate()
    {

    } 
}