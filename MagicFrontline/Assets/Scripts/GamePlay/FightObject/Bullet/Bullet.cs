using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float OffsetAngle=-90;
    protected bool mIsDead;
    protected int mMoveSpeed=1200;
    protected double mLiveTime;
    protected double mMaxLifeTime;
    protected Tower mSource;
    protected AttackEffectId mAttackEffectId;
    //子弹伤害
    protected int mPoints=1;
    protected List<Enemy> mIgnoreList;
    //能否穿透
    protected bool mIsPenetrate;
    protected MyCollider mCollider;
    protected Animator mAnimator;

    protected virtual void Init(Tower source,int points,AttackEffectId attackEffectId=AttackEffectId.None)
    {
        mAttackEffectId=attackEffectId;
        mCollider=new MyCollider(GetComponent<PolygonCollider2D>());
        mAnimator=GetComponent<Animator>();
        mSource=source;
        mPoints=points;
        mLiveTime=0;
        mMaxLifeTime=3;
        mIsPenetrate=false;    
        mIgnoreList=new List<Enemy>();
    }

    public virtual void OnUpdate()
    {
        mCollider.OnUpdate();
        mLiveTime+=Time.deltaTime;
        if(mLiveTime>=mMaxLifeTime)
        {
            mIsDead=true;
        }
    }

    public void PlayDestroyAnimation()
    {
        gameObject.SetActive(false);
        DOVirtual.DelayedCall(3,()=>
        {
            DOTween.Kill(gameObject);
            Destroy(gameObject);
        });
    }

    //子弹碰撞敌人造成伤害
    public virtual void OnColliderEnemy(Enemy target)
    {
        if(mIsPenetrate)
        {
            if(mIgnoreList.Contains(target))
            {
                return;
            }
            else
            {
                mIgnoreList.Add(target);
                target.Damage(mPoints,mAttackEffectId);
            }
        }
        else
        {
            mIsDead=true;
            target.Damage(mPoints,mAttackEffectId);
        }
    }

    public bool IsDead(){return mIsDead;}
    public MyCollider GetCollider(){return mCollider;}
    public int GetPoints(){return mPoints;}
    protected string GetAnimationName(string bulletName,int level)
    {
        return bulletName+"0"+level;
    }
}