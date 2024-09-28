using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool mIsDead;
    protected int mMoveSpeed=1200;
    protected double mLiveTime;
    protected double mMaxLifeTime;
    protected Tower mSource;
    //子弹伤害
    protected int mPoints=1;
    protected List<Enemy> mIgnoreList;
    //能否穿透
    protected bool mIsPenetrate;
    protected MyCollider mCollider;

    protected virtual void Init(Tower source,int points)
    {
        mCollider=new MyCollider(GetComponent<PolygonCollider2D>());
        mSource=source;
        mPoints=points;
        mLiveTime=0;
        mMaxLifeTime=3;
        mIsPenetrate=false;
        mIgnoreList=new List<Enemy>();
    }

    public void OnUpdate()
    {
        mCollider.OnUpdate();
        mLiveTime+=Time.deltaTime;
        if(mLiveTime>=mMaxLifeTime)
        {
            mIsDead=true;
        }
    }
}