using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float OffsetAngle=-90;
    protected bool mIsDead;
    protected int mMoveSpeed=2000;
    protected double mLiveTime;
    protected double mMaxLifeTime;
    protected Tower mSource;
    protected StatusEffectId mStatusEffectId;
    //子弹伤害
    protected int mPoints=1;
    protected List<Enemy> mIgnoreList;
    //能否穿透
    protected bool mIsPenetrate;
    protected MyCollider mCollider;
    protected Animator mAnimator;
    protected Enemy mTarget;
    protected bool mHasAttack;

    protected virtual void Init(Tower source,Enemy target,int points,StatusEffectId attackEffectId=StatusEffectId.None)
    {
        mStatusEffectId=attackEffectId;
        mCollider=new MyCollider(GetComponent<PolygonCollider2D>());
        mAnimator=GetComponent<Animator>();
        mSource=source;
        mTarget=target;
        mPoints=points;
        mLiveTime=0;
        mMaxLifeTime=5;
        mIsPenetrate=false;    
        mIgnoreList=new List<Enemy>();
        mAnimator.Play("Bullet"+source.GetName()+source.GetLevel());
    }

    public virtual void OnUpdate()
    {
        if(mIsDead)
        {
            return;
        }
        mCollider.OnUpdate();
        mLiveTime+=Time.deltaTime;
        if(mLiveTime>=mMaxLifeTime)
        {
            mIsDead=true;
        }
        //目标敌人死亡
        if(mTarget != null && mTarget.IsDead())
        {
            //重新获取一个敌人替换为目标
            mTarget=FightUtility.GetTargetEnemy(transform.position,100);
        }
        if(mTarget==null)
        {
            FightUtility.MoveTowardsRotation(gameObject,mMoveSpeed,OffsetAngle);
        }
    }

    public void PlayDestroyAnimation()
    {
        gameObject.SetActive(false);
        DOVirtual.DelayedCall(3,()=>
        {
            DOTween.Kill(gameObject);
            Destroy(gameObject);
        },false);
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
                target.Damage(mPoints,mStatusEffectId);
            }
        }
        else
        {
            if(!mHasAttack)
            {
                mHasAttack=true;
                mIsDead=true;
                target.Damage(mPoints,mStatusEffectId);
            }
        }
    }

    public bool IsDead(){return mIsDead;}
    public MyCollider GetCollider(){return mCollider;}
    public int GetPoints(){return mPoints;}
}