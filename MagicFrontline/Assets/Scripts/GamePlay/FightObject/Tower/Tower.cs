using System;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected const float OffsetAngle=-90;
    public const int MaxLevel=3;
    protected int mAttack=10;
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
    public virtual void Init(int level,Vector3Int position,Tuple<int ,float> property)
    {
        mLevel=level;
        mPosition=position;
        transform.position=FightModel.GetCurrent().GetMap().LogicToWorldPosition(mPosition);
        mAttack=property.Item1;
        mAttackinterval=property.Item2;
        mAnimator=GetComponent<Animator>();
    }
    public virtual void OnUpdate()
    {
        mCanShoot=HasTarget();
        //朝向目标
        if(mCanShoot)
        {
            Vector3 direction=(mTargetEnemy.transform.position-transform.position).normalized;
            transform.rotation=FightUtility.DirectionToRotation(direction,OffsetAngle);
        }
        //射击
        if(mDefaultShootTime>=mAttackinterval&&mCanShoot)
        {
            mShootCallback();
            mDefaultShootTime=0;
        }
        mDefaultShootTime+=Time.deltaTime;
    }

    protected bool HasTarget()
    {
        mTargetEnemy=FightUtility.GetTargetEnemy(this,mShootRange);
        return mTargetEnemy!=null;
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
    public int GetLevel(){return mLevel;}
    public Vector3Int GetPosition(){return mPosition;}
    public int GetShootRange(){return mShootRange;}
}