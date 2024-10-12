using UnityEngine;

//减速子弹 -- 跟踪子弹
public class BulletFrozen : Bullet
{
    private Enemy mTarget;
    public static BulletFrozen Create(Tower tower,Enemy target,int points)
    {
        GameObject bulletPrefab=Resources.Load<GameObject>("FightObject/Bullet/BulletFrozen");
        GameObject bulletObject=GameObject.Instantiate(bulletPrefab);
        BulletFrozen bullet=bulletObject.AddComponent<BulletFrozen>();
        bullet.Init(tower,target,points);
        
        return bullet;
    }
    protected void Init(Tower tower,Enemy target,int points)
    {
        base.Init(tower,points,AttackEffectId.MoveSpeedDecrease);
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