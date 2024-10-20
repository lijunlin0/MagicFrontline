using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

public class EffectArea:MonoBehaviour
{
    private Animator mAnimator;
    private SpriteRenderer mSpriteRenderer;
    private MyCollider mCollider;
    private Callback<Enemy> mCollideCallback;
    private Callback mColliderEnabledCallback;
    private string mAnimationName;

    public static EffectArea Create(string prefabName,string animationName,Vector3 position,Callback<Enemy> collideCallback,float scale=0)
    {
        GameObject prefab=Resources.Load<GameObject>("FightObject/Area/"+prefabName);
        GameObject areaObject=Instantiate(prefab,position,Quaternion.identity);
        if(scale!=0)
        {
            areaObject.transform.localScale=new Vector3(scale,scale,1);
        }
        EffectArea area=areaObject.AddComponent<EffectArea>();
        area.Init(animationName,collideCallback);
        return area;
    }
    private void Init(string animationName,Callback<Enemy> collideCallback)
    {
        mAnimationName=animationName;
        mAnimator=GetComponent<Animator>();
        mSpriteRenderer=GetComponent<SpriteRenderer>();
        mAnimator.speed=0;
        mAnimator.Play(mAnimationName);
        mAnimator.speed=1;
        mCollider=new MyCollider(GetComponent<PolygonCollider2D>());
        mCollideCallback=collideCallback;
        
    }

    private void Update()
    {
        //防止播放其他动画
         AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(mAnimationName))
        {
            if (stateInfo.normalizedTime >= 0.95f)
            {
                mSpriteRenderer.enabled=false;
            }
        }
        mCollider.OnUpdate();
        if(mCollider.IsEnable()&&mColliderEnabledCallback!=null)
        {
            mColliderEnabledCallback();
            mColliderEnabledCallback=null;
        }
    }

    public void SetColliderEnabledCallback(Callback callback)
    {
        mColliderEnabledCallback=callback;
    }

    public void Collide()
    {
        List<Enemy> enemies=FightModel.GetCurrent().GetEnemies();
        foreach(Enemy enemy in enemies)
        {
            MyCollider collider=enemy.GetCollider();
            if(CollisionHelper.IsColliding(mCollider,collider))
            {
                mCollideCallback(enemy);
            }
        }
    }
    public float GetAnimationDuration()
    {
        return mAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
    public void PlayDestroyAnimation(float duration)
    {
        Destroy(gameObject,duration);
    }

    public void OnDeathAnimationEnd()
    {

    }
}