using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//光波宝石
public class LightwaveStone : Tower
{
    private float mBulletDefaultScale=1.5f;
    private float mBulletScaleAddition=0.25f;
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
     public void Init(int level,Vector3Int position,Tuple<int,float,int,int> property)
    {
        base.Init(level,position,property,"LightwaveStone");
        mIsRotate=false;
        mAnimationOffsetTime=(DefaultAnimationDuration-AnimationDurationReduce*(mLevel-1))/2;
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
            Vector3 createPosition=new Vector3(transform.position.x,transform.position.y,-1);
            EffectArea area=EffectArea.Create("BulletLightwaveStoneArea",animationName,createPosition,(Enemy Enemy)=>
            {
                Enemy.Damage(mAttack);
            },bulletScale);
            area.SetColliderEnabledCallback(()=>
            {
                area.PlayDestroyAnimation(area.GetAnimationDuration());
                DOVirtual.DelayedCall(area.GetAnimationDuration()-0.25f,()=>
                {
                    area.Collide();
                },false);
                
            });
        };
    }
}
