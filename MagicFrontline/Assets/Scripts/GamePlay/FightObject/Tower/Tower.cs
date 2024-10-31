using System;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected const float DefaultAnimationDuration=0.5f;
    protected const float AnimationDurationReduce=0.166f;
    protected const float OffsetAngle=-90;
    public const int MaxLevel=3;
    protected string mName;
    protected StatusEffectId mStatusEffectId;
    protected int mAttack=10;
    protected int mCreatePrice;
    protected float mAttackinterval=1;
    protected bool mIsDead=false;
    protected int mLevel=1;
    protected Callback mShootCallback;
    protected int mShootRange=0;
    protected float mDefaultShootTime=0;
    protected bool mCanShoot=true;
    protected Enemy mTargetEnemy;
    protected Vector3Int mPosition;
    protected Animator mAnimator;
    protected float mAnimationOffsetTime=0;
    //是否需要在攻击时朝向敌人
    protected bool mIsRotate=false;
    public virtual void Init(int level,Vector3Int position,Tuple<int,float,int,int> property,string name,StatusEffectId statusEffectId=StatusEffectId.None)
    {
        mLevel=level;
        mName=name;
        mPosition=position;
        mStatusEffectId=statusEffectId;
        transform.position=FightModel.GetCurrent().GetMap().LogicToWorldPosition(mPosition);
        transform.position=new Vector3(transform.position.x,transform.position.y,-2);
        mAttack=property.Item1;
        mAttackinterval=property.Item2;
        mShootRange=property.Item3;
        mCreatePrice=property.Item4;
        mAnimator=GetComponent<Animator>();
        PlayIdleAnimation();
    }
    public virtual void OnUpdate()
    {
        mCanShoot=HasTarget();
        //朝向目标
        if(mIsRotate&&mCanShoot)
        {
            Vector3 direction = (mTargetEnemy.GetCenterPosition() - transform.position).normalized;
            Quaternion targetRotation = FightUtility.DirectionToRotation(direction, OffsetAngle);

            float degreesPerSecond = 240f; // 控制旋转速度，可以根据需要调整
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, degreesPerSecond * Time.deltaTime);
            float angleDifference=Quaternion.Angle(transform.rotation,targetRotation);
            float shootAngleThreshold = 5f; //小于这个角度可以发射
            if (angleDifference <= shootAngleThreshold)
            {
                // 射击
                if (mDefaultShootTime >= mAttackinterval)
                {
                    PlayAttackAnimation();
                    DOVirtual.DelayedCall(mAnimationOffsetTime,()=>
                    {
                        mShootCallback();
                    },false);
                    mDefaultShootTime = 0;
                }
            }
        }
        else if(mCanShoot)
        {
            // 射击
            if (mDefaultShootTime >= mAttackinterval)
            {
                PlayAttackAnimation();
                DOVirtual.DelayedCall(mAnimationOffsetTime,()=>
                {
                    mShootCallback();
                },false);
                mDefaultShootTime = 0;
            }
        }

        mDefaultShootTime+=Time.deltaTime;
    }

    protected bool HasTarget()
    {
        mTargetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange,mStatusEffectId);
        return mTargetEnemy!=null;
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
    public int GetLevel(){return mLevel;}
    public Vector3Int GetPosition(){return mPosition;}
    public int GetShootRange(){return mShootRange;}
    protected void PlayIdleAnimation()
    {
        mAnimator.Play(mName+"Idle"+mLevel.ToString());
    }
    protected void PlayAttackAnimation()
    {
        mAnimator.Play(mName+"Shoot"+mLevel.ToString());
    }

    public string GetName(){return mName;}
}