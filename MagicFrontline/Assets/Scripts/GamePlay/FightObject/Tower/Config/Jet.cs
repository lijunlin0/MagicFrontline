using System;
using UnityEngine;

//喷石嘴
public class Jet : Tower
{
    
    public static Jet Create(int level,Vector3Int position,Quaternion rotation = default)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/Jet/Jet");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.rotation=rotation;
        Jet jet=towerObject.AddComponent<Jet>();
        var property=TowerUtility.GetBasePropertySheet("Jet",level);
        jet.Init(level,position,property);
        return jet;
    }
     public void Init(int level,Vector3Int position,Tuple<int,float,int,int> property)
    {
        base.Init(level,position,property,"Jet");
        mIsRotate=true;
        mAnimationOffsetTime=(DefaultAnimationDuration-AnimationDurationReduce*(mLevel-1))/2;
        mShootCallback=()=>
        {
            Enemy targetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange);
            if(targetEnemy==null)
            {
                return;
            }
            BulletJet bulletJet=BulletJet.Create(this,targetEnemy,mAttack);
            bulletJet.transform.position=transform.position;
            Vector3 direction=(targetEnemy.transform.position-bulletJet.transform.position).normalized;
            bulletJet.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle);
            FightModel.GetCurrent().AddBullet(bulletJet);
            if(mLevel>1)
            {
                BulletJet bulletJet2=BulletJet.Create(this,targetEnemy,mAttack);
                bulletJet2.transform.position=transform.position;
                bulletJet2.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle+180);
                FightModel.GetCurrent().AddBullet(bulletJet2);
            }
            if(mLevel>2)
            {
                BulletJet bulletJet3=BulletJet.Create(this,targetEnemy,mAttack);
                bulletJet3.transform.position=transform.position;
                bulletJet3.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle+90);
                FightModel.GetCurrent().AddBullet(bulletJet3);

                BulletJet bulletJet4=BulletJet.Create(this,targetEnemy,mAttack);
                bulletJet4.transform.position=transform.position;
                bulletJet4.transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle+270);
                FightModel.GetCurrent().AddBullet(bulletJet4);
            }
        };
    }
}
