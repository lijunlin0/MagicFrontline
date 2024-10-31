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
    public const float MoveSpeedReductionPercent=40;
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
    protected GameObject mDisplay;
    protected GameObject mShadow;
    protected GameObject mCenter;
    protected GameObject mTop;
    protected int mAttackPoints;
    //是否正在减速
    protected bool mIsMoveSpeedDebuff=false;
    protected float mMoveDistance;
    //现在的路径点下标
    protected int mTurningPointIndex;
    protected int mHealth;
    protected float mMoveSpeed=200;
    public bool isDamage=false;
    protected bool mIsArrive=false;
    protected int mCoinCount;
    private Dictionary<StatusEffectId, Tween> mActiveStatusEffects;
    private bool mFlipX=false;
    private bool mIsOnBridge=false;
    protected virtual void Init(EnemyId enemyId,Property baseProperty)
    {
        mEnemyId=enemyId;
        mProperty=baseProperty;
        mDisplay=transform.Find("Display").gameObject;
        mShadow=transform.Find("Display/Shadow").gameObject;
        mShadow.transform.localPosition =new Vector3(mShadow.transform.localPosition.x,mShadow.transform.localPosition.y,0.5f);
        mCenter=transform.Find("Center").gameObject;
        mTop=transform.Find("Top").gameObject;
        mHealth=mProperty.GetBaseHealth();
        mMoveSpeed=baseProperty.GetBaseMoveSpeed();
        mCoinCount=baseProperty.GetBaseCoinCount();
        Debug.Log("持有金币:"+mCoinCount);
        mActiveStatusEffects=new Dictionary<StatusEffectId, Tween>();
        mAnimator=mDisplay.GetComponent<Animator>();

        //除了boss外伤害都是1
        mAttackPoints=IsBoss()?5:1;
        mTurningPointIndex=1;
        mCollider=new MyCollider(mDisplay.GetComponent<PolygonCollider2D>());
        mSpriteRenderer=mDisplay.GetComponent<SpriteRenderer>();
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
            mIsArrive=true;
            mIsDead=true;
            FightModel.GetCurrent().GetHurt(mAttackPoints);
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
            if(mTurningPointIndex<mTurningPointList.Count&&mTurningPointList[mTurningPointIndex].x<mTurningPointList[mTurningPointIndex-1].x)
            {
                mFlipX=true;
            }
            else if(mTurningPointIndex+1<mTurningPointList.Count)
            {
                mFlipX=mTurningPointList[mTurningPointIndex].x>mTurningPointList[mTurningPointIndex+1].x?true:false;
            }
            //翻转
            transform.localScale=new Vector3(mFlipX?-1:1,1,1);
        }
        float moveDistance=mMoveSpeed*Time.deltaTime;
        transform.position+=direction.normalized*moveDistance;
        mMoveDistance+=moveDistance;
    }

    public void WalkOnBridge()
    {
        mIsOnBridge=true;
        List<Vector3> bridgePointList=FightModel.GetCurrent().GetMap().GetBridgePointList();
        for(int i=0;i<bridgePointList.Count;i++)
        {
            mTurningPointList.Insert(mTurningPointIndex+i,bridgePointList[i]);
        }
    }

    public bool IsAlreadyOnBridge(){return mIsOnBridge;}

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
        if(statusEffectId==StatusEffectId.MoveSpeedDecrease)
        {
            //重置持续时间
            if(HasStatusEffect(statusEffectId))
            {
                mActiveStatusEffects[statusEffectId].Restart();
            }
            else
            {
                mMoveSpeed*=(100-MoveSpeedReductionPercent)/100;
                mStatusEffectIdList.Add(statusEffectId);
                Tween statusEffectTween=DOVirtual.DelayedCall(MoveSpeedReductionDuration,()=>
                {
                    mMoveSpeed=mProperty.GetBaseMoveSpeed();
                    mStatusEffectIdList.Remove(statusEffectId);
                    mActiveStatusEffects.Remove(statusEffectId);
                },false);
                mActiveStatusEffects[statusEffectId] = statusEffectTween;
            }
            
        }
    }

    public bool HasStatusEffect(StatusEffectId statusEffectId){return mStatusEffectIdList.Contains(statusEffectId);}

    public void PlayDestroyAnimation()
    {
        if(!mIsArrive)
        {
            Coin.Create(transform.position,mCoinCount);
        }
        mAnimator.Play(mEnemyId.ToString()+"Death");
        mShadow.GetComponent<SpriteRenderer>().DOFade(0,0.4f);
        mSpriteRenderer.DOFade(0,0.4f).OnComplete(()=>
        {
            DOTween.Kill(gameObject);
            DOVirtual.DelayedCall(5,()=>
            {
                mCollider.GetCollider().enabled=false;
                Destroy(gameObject);  
            },false);
        });
    }

    public float GetMoveProgress()
    {
        return mMoveDistance/(FightModel.GetCurrent().GetMap().GetWayPointCount()-1)*Map.TileSize;
    }

    public Vector3 GetCenterPosition(){return mCenter.transform.position;}
    public Vector3 GetTopPosition(){return mTop.transform.position;}

    public Property GetProperty(){return mProperty;}
    public bool IsDead(){return mIsDead;}
    public MyCollider GetCollider(){return mCollider;}
    public bool IsBoss()
    {
        return mEnemyId==EnemyId.Boss1||mEnemyId==EnemyId.Boss2||mEnemyId==EnemyId.Boss3;
    }
}