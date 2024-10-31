using UnityEngine;

//爆炸弹弓子弹 -- 跟踪子弹
public class BulletExplosiveSlingshot : Bullet
{
    public static BulletExplosiveSlingshot Create(Tower tower,Enemy target,int points)
    {
        GameObject bulletPrefab=Resources.Load<GameObject>("FightObject/Bullet/BulletExplosiveSlingshot");
        GameObject bulletObject=GameObject.Instantiate(bulletPrefab);
        BulletExplosiveSlingshot bullet=bulletObject.AddComponent<BulletExplosiveSlingshot>();
        bullet.Init(tower,target,points);
        return bullet;
    }
    protected void Init(Tower tower,Enemy target,int points)
    {
        base.Init(tower,target,points);
        mMoveSpeed=1400;
        mAnimator.Play("BulletExplosiveSlingshot"+tower.GetLevel().ToString());
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(mTarget!=null)
        {
            FightUtility.MoveTowardsTarget(gameObject,mTarget,mMoveSpeed,OffsetAngle);
        }
    }
    public override void OnColliderEnemy(Enemy target)
    {
        mIsDead=true;
        string animationName="BulletExplosiveSlingshotImpact"+mSource.GetLevel().ToString();
        EffectArea area=EffectArea.Create("ExplosiveSlingshotArea",animationName,target.transform.position,(Enemy Enemy)=>
        {
            Enemy.Damage(mPoints);
        });
        area.SetColliderEnabledCallback(()=>
        {
            area.PlayDestroyAnimation(area.GetAnimationDuration());
            area.Collide();
        });
    }
}