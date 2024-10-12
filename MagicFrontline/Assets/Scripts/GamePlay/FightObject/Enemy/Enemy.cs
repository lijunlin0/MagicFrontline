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
public enum AttackEffectId
{
    None=0,                 
    MoveSpeedDecrease,      //降低移动速度
}

public class Enemy : MonoBehaviour
{
    private EnemyId mEnemyId;
    protected bool mIsDead=false;
    protected Property mProperty;
    protected Animator mAnimator;
    protected HealthBar mHealthBar;
    protected List<Vector3> mTurningPointList;
    protected SpriteRenderer mSpriteRenderer;
    protected MyCollider mCollider;
    protected float mMoveDistance;
    //现在的路径点下标
    protected int mTurningPointIndex;
    protected int mHealth;
    protected int mMoveSpeed=200;
    public bool isDamage=false;
    protected virtual void Init(EnemyId enemyId,Property baseProperty)
    {
        mEnemyId=enemyId;
        mProperty=baseProperty;
        mHealth=mProperty.GetBaseHealth();
        mMoveSpeed=baseProperty.GetBaseMoveSpeed();
        mTurningPointIndex=1;
        mCollider=new MyCollider(GetComponent<PolygonCollider2D>());
        mSpriteRenderer=GetComponent<SpriteRenderer>();
        mHealthBar=HealthBar.Create(this);
        mTurningPointList=new List<Vector3>();
        List<Vector3Int> logicPointList=FightModel.GetCurrent().GetMap().GeturningPointList();
        foreach(Vector3Int logicPosition in logicPointList)
        {
            mTurningPointList.Add(FightModel.GetCurrent().GetMap().LogicToWorldPosition(logicPosition));
        }
    }

    public void OnUpdate()
    {
        if(mIsDead==true)
        {
            return;
        }
        mCollider.OnUpdate();
        Move();

        //掉血---测试代码
        return;
        if(isDamage)
        {
            return;
        }
        DOVirtual.DelayedCall(1,()=>
        {
            Damage(10);
            isDamage=false;
        });
        isDamage=true;
        
    }

    private void Move()
    {
        if(mTurningPointIndex>=mTurningPointList.Count)
        {
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
    public void Damage(int points,AttackEffectId attackEffectId=AttackEffectId.None)
    {
        mProperty.ChangeCurrentHealth(-points);
        mHealth=mProperty.GetCurrentHealth();
        mHealthBar.OnHealthChanged();
        if(mHealth<=0)
        {
            mIsDead=true;
        }
    }
    public int GetHealth(){return mHealth;}
    
    public void HaveAttackEffect(AttackEffectId attackEffectId)
    {
        if(attackEffectId==AttackEffectId.MoveSpeedDecrease)
        {
            
        }
    }

    public void PlayDestroyAnimation()
    {
        mCollider.GetCollider().enabled=false;
        //mAnimator.Play(mEnemyId.ToString()+"Death");
        mSpriteRenderer.DOFade(0,1).OnComplete(()=>
        {
            DOTween.Kill(gameObject);
            Destroy(gameObject,3);  
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