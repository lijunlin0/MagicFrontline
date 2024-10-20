using System;
using UnityEditor;
using UnityEngine;

//弓箭
public class Bow : Tower
{
    public static Bow Create(int level,Vector3Int position,Quaternion rotation = default)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/Bow/Bow");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.rotation=rotation;
        Bow bow=towerObject.AddComponent<Bow>();
        var property=TowerUtility.GetBasePropertySheet("Bow",level);
        bow.Init(level,position,property);
        return bow;
    }
    public void Init(int level,Vector3Int position,Tuple<int,float,int,int> property)
    {
        base.Init(level,position,property,"Bow");
        mIsRotate=true;
        mAnimationOffsetTime=0.25f;
        mShootCallback=()=>
        {
            Enemy targetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange);
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
