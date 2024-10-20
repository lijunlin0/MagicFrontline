using System;
using UnityEditor;
using UnityEngine;

//减速弓箭
public class FrozenBow : Tower
{
    public static FrozenBow Create(int level,Vector3Int position,Quaternion rotation = default)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/FrozenBow/FrozenBow");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.rotation=rotation;
        FrozenBow frozenBow=towerObject.AddComponent<FrozenBow>();
        var property=TowerUtility.GetBasePropertySheet("FrozenBow",level);
        frozenBow.Init(level,position,property);
        return frozenBow;
    }
    public void Init(int level,Vector3Int position,Tuple<int,float,int> property)
    {
        base.Init(level,position,property,"FrozenBow",StatusEffectId.MoveSpeedDecrease);
        mAnimationOffsetTime=0.25f;
        mIsRotate=true;
        mShootCallback=()=>
        {
            Enemy targetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange,mStatusEffectId);
            if(targetEnemy==null)
            {
                return;
            }
            BulletFrozenBow bulletFrozenBow=BulletFrozenBow.Create(this,targetEnemy,mAttack);
            bulletFrozenBow.transform.position=transform.position;
            
            Vector3 direction=(targetEnemy.transform.position-bulletFrozenBow.transform.position).normalized;
            bulletFrozenBow.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle);
            FightModel.GetCurrent().AddBullet(bulletFrozenBow);
        };
    }
}
