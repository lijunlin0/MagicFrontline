using System;
using UnityEngine;

//爆炸弹弓
public class ExplosiveSlingshot : Tower
{
    
    public static ExplosiveSlingshot Create(int level,Vector3Int position,Quaternion rotation = default)
    {
         GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/ExplosiveSlingshot/ExplosiveSlingshot");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.rotation=rotation;
        ExplosiveSlingshot explosiveSlingshot=towerObject.AddComponent<ExplosiveSlingshot>();
        var property=TowerUtility.GetBasePropertySheet("ExplosiveSlingshot",level);
        explosiveSlingshot.Init(level,position,property);
        return explosiveSlingshot;
    }
    public void Init(int level,Vector3Int position,Tuple<int,float,int,int> property)
    {
        base.Init(level,position,property,"ExplosiveSlingshot");
        mIsRotate=true;
        mAnimationOffsetTime=(DefaultAnimationDuration-AnimationDurationReduce*(mLevel-1))/2;
        mShootCallback=()=>
        {
            Enemy targetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange);
            if(targetEnemy==null)
            {
                return;
            }
            BulletExplosiveSlingshot bullet=BulletExplosiveSlingshot.Create(this,targetEnemy,mAttack);
            bullet.transform.position=transform.position;
            
            Vector3 direction=(targetEnemy.transform.position-bullet.transform.position).normalized;
            bullet.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle);
            FightModel.GetCurrent().AddBullet(bullet);
        };
    }
}
