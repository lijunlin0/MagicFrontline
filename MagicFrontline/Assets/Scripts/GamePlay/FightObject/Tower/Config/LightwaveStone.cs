using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//光波宝石
public class LightwaveStone : Tower
{
    private float mBulletDefaultScale=3;
    private float mBulletScaleAddition=0.5f;
    public static LightwaveStone Create(int level,Vector3Int position,Quaternion rotation = default)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/LightwaveStone/LightwaveStone");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.rotation=rotation;
        LightwaveStone lightwaveStone=towerObject.AddComponent<LightwaveStone>();
        var property=TowerUtility.GetBasePropertySheet("LightwaveStone",level);
        lightwaveStone.Init(level,position,property);
        return lightwaveStone;
    }
     public void Init(int level,Vector3Int position,Tuple<int,float,int> property)
    {
        base.Init(level,position,property,"LightwaveStone");
        mIsRotate=false;
        mAnimationOffsetTime=0.25f;
        mShootCallback=()=>
        {
            Enemy targetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange);
            if(targetEnemy==null)
            {
                return;
            }
            Debug.Log("光波等级:"+GetLevel());
            float bulletScale=mBulletDefaultScale+(GetLevel()-1)*mBulletScaleAddition;
            string animationName="BulletLightwaveStone2";
            EffectArea area=EffectArea.Create("BulletLightwaveStoneArea",animationName,transform.position,(Enemy Enemy)=>
            {
                Enemy.Damage(mAttack);
            },bulletScale);
            area.SetColliderEnabledCallback(()=>
            {
                area.PlayDestroyAnimation(area.GetAnimationDuration());
                DOVirtual.DelayedCall(area.GetAnimationDuration()-0.4f,()=>
                {
                    area.Collide();
                });
                
            });
        };
    }
}
