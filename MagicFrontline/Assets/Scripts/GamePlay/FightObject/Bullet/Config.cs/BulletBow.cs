using UnityEngine;

//弓箭子弹 -- 跟踪子弹
public class BulletBow : Bullet
{
    private Enemy mTarget;
    public static BulletBow Create(Tower tower,Enemy target,int points)
    {
        GameObject bulletPrefab=Resources.Load<GameObject>("FightObject/Bullet/BulletBow");
        GameObject bulletObject=GameObject.Instantiate(bulletPrefab);
        BulletBow bullet=bulletObject.AddComponent<BulletBow>();
        bullet.Init(tower,target,points);
        return bullet;
    }
    protected void Init(Tower tower,Enemy target,int points)
    {
        base.Init(tower,points);
        mTarget=target;
        mAnimator.Play(GetAnimationName("BulletBow",tower.GetLevel()));
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(mIsDead)
        {
            return;
        }
        FightUtility.MoveTowardsTarget(gameObject,mTarget.gameObject,mMoveSpeed,OffsetAngle);
    }
}