using UnityEngine;

public class MyCollider
{
    private Collider2D mCollider;
    private bool mEnable;
    private float mCreateTime;

    public MyCollider(Collider2D collider)
    {
        mCreateTime=0;
        mCollider = collider;
        mEnable=false;
    }
    public void OnUpdate()
    {
        mCreateTime+= Time.deltaTime;
        if(mCreateTime>=0.05)
        {
            mEnable = true;
        }
    }

    public Collider2D GetCollider(){return mCollider;}
    public bool IsEnable(){return mEnable;}
}