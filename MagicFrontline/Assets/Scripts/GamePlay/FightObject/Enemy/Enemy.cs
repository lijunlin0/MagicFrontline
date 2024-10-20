using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum EnemyId
{
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,
    Enemy5,
    Enemy6,
    Enemy7,
    Enemy8,
    Enemy9,
    Enemy10,
    Boss1,
    Boss2,
    Boss3,
}
public enum StatusEffectId
{
    None=0,                 
    MoveSpeedDecrease,      //降低移动速度
}

public class Enemy : MonoBehaviour
{
    public const float MoveSpeedReductionPercent=30;
    public const float MoveSpeedReductionDuration=2;
    private EnemyId mEnemyId;
    protected bool mIsDead=false;
    protected Property mProperty;
    protected Animator mAnimator;
    protected HealthBar mHealthBar;
    protected List<StatusEffectId> mStatusEffectIdList;
    protected List<Vector3> mTurningPointList;
    protected SpriteRenderer mSpriteRenderer;
    protected MyCollider mCollider;
    //是否正在减速
    protected bool mIsMoveSpeedDebuff=false;
    protected float mMoveDistance;
    //现在的路径点下标
    protected int mTurningPointIndex;
    protected int mHealth;
    protected float mMoveSpeed=200;
    public bool isDamage=false;
    protected virtual void Init(EnemyId enemyId,Property baseProperty)
    {
        mEnemyId=enemyId;
        mProperty=baseProperty;
        mHealth=mProperty.GetBaseHealth();
        mAnimator=GetComponent<Animator>();
        mMoveSpeed=baseProperty.GetBaseMoveSpeed();
        mTurningPointIndex=1;
        mCollider=new MyCollider(GetComponent<PolygonCollider2D>());
        mSpriteRenderer=GetComponent<SpriteRenderer>();
        mHealthBar=HealthBar.Create(this);
        mStatusEffectIdList=new List<StatusEffectId>();
        mTurningPointList=new List<Vector3>();
        List<Vector3Int> logicPointList=FightModel.GetCurrent().GetMap().GetTurningPointList();
        foreach(Vector3Int logicPosition in logicPointList)
        {
            mTurningPointList.Add(FightModel.GetCurrent().GetMap().LogicToWorldPosition(logicPosition));
        }
        Debug.Log("敌人血量:"+mHealth);
    }

    public void OnUpdate()
    {
        if(mIsDead==true)
        {
            return;
        }
        mCollider.OnUpdate();
        Move();
    }

    private void Move()
    {
        if(mTurningPointIndex>=mTurningPointList.Count)
        {
            mIsDead=true;
            return;
        }
        
        Vector3 nextTurningPoint=mTurningPointList[mTurningPointIndex];

        //根据路径点移动
        Vector3 direction=nextTurningPoint-transform.position;
        direction.z=0;
        float distance=direction.sqrMagnitude;
        if(distance<50)
        {
            mTurningPointIndex++;
            //根据路径点调整朝向
            if(mTurningPointIndex+1<mTurningPointList.Count)
            {
                mSpriteRenderer.flipX=mTurningPointList[mTurningPointIndex].x>mTurningPointList[mTurningPointIndex+1].x?true:false;
            }
        }
        float moveDistance=mMoveSpeed*Time.deltaTime;
        transform.position+=direction.normalized*moveDistance;
        mMoveDistance+=moveDistance;
    }

    //受击
    public void Damage(int points,StatusEffectId statusEffectId=StatusEffectId.None)
    {
        Color color=mSpriteRenderer.color;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(mSpriteRenderer.material.DOColor(Color.red,0.1f));
        sequence.Append(mSpriteRenderer.material.DOColor(color,0.1f));
        sequence.Play();
        mProperty.ChangeCurrentHealth(-points);
        mHealth=mProperty.GetCurrentHealth();
        mHealthBar.OnHealthChanged();
        if(statusEffectId!=StatusEffectId.None)
        {
            AddStatusEffect(statusEffectId);
        }
        if(mHealth<=0)
        {
            mIsDead=true;
        }
    }
    public int GetHealth(){return mHealth;}
    
    public void AddStatusEffect(StatusEffectId statusEffectId)
    {
        if(!HasStatusEffect(statusEffectId)&&statusEffectId==StatusEffectId.MoveSpeedDecrease)
        {
            mMoveSpeed*=(100-MoveSpeedReductionPercent)/100;
            mStatusEffectIdList.Add(statusEffectId);
            DOVirtual.DelayedCall(MoveSpeedReductionDuration,()=>
            {
                mMoveSpeed=mProperty.GetBaseMoveSpeed();
                mStatusEffectIdList.Remove(statusEffectId);
            });
        }
    }

    public bool HasStatusEffect(StatusEffectId statusEffectId){return mStatusEffectIdList.Contains(statusEffectId);}

    public void PlayDestroyAnimation()
    {
        mAnimator.Play(mEnemyId.ToString()+"Death");
        mSpriteRenderer.DOFade(0,0.4f).OnComplete(()=>
        {
            DOTween.Kill(gameObject);
            DOVirtual.DelayedCall(5,()=>
            {
                mCollider.GetCollider().enabled=false;
                Destroy(gameObject);  
            });
        });
    }

    public float GetMoveProgress()
    {
        return mMoveDistance/(FightModel.GetCurrent().GetMap().GetWayPointCount()-1)*Map.TileSize;
    }

    public Property GetProperty(){return mProperty;}
    public bool IsDead(){return mIsDead;}
    public MyCollider GetCollider(){return mCollider;}
}