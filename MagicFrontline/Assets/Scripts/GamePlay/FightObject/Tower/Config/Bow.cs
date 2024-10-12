using System;
using UnityEditor;
using UnityEngine;

public class Bow : Tower
{
    public static Bow Create(int level,Vector3Int position)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/Bow/Bow");
        GameObject towerObject=Instantiate(towerPrefab);
        Bow bow=towerObject.AddComponent<Bow>();
        var property=TowerUtility.GetBasePropertySheet("Bow",level);
        bow.Init(level,position,property);
        return bow;
    }
    public override void Init(int level,Vector3Int position,Tuple<int,float> property)
    {
        base.Init(level,position,property);
        mShootRange=800;
        mAnimator.speed=0;
        mShootCallback=()=>
        {
            Enemy targetEnemy=FightUtility.GetTargetEnemy(this,mShootRange);
            if(targetEnemy==null)
            {
                return;
            }
            BulletBow bulletBow=BulletBow.Create(this,targetEnemy,mAttack);
            bulletBow.transform.position=transform.position;
            
            Vector3 direction=(targetEnemy.transform.position-bulletBow.transform.position).normalized;
            bulletBow.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle);
            FightModel.GetCurrent().AddBullet(bulletBow);
        };
    }
}
