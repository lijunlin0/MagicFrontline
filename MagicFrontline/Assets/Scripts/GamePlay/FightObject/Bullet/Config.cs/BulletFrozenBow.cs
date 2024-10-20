using UnityEngine;

//减速子弹 -- 跟踪子弹
public class BulletFrozenBow : Bullet
{
    public static BulletFrozenBow Create(Tower tower,Enemy target,int points)
    {
        GameObject bulletPrefab=Resources.Load<GameObject>("FightObject/Bullet/BulletFrozenBow");
        GameObject bulletObject=GameObject.Instantiate(bulletPrefab);
        BulletFrozenBow bullet=bulletObject.AddComponent<BulletFrozenBow>();
        bullet.Init(tower,target,points);
        
        return bullet;
    }
    protected void Init(Tower tower,Enemy target,int points)
    {
        base.Init(tower,target,points,StatusEffectId.MoveSpeedDecrease);
        mAnimator.Play("BulletFrozenBow"+tower.GetLevel().ToString());
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(mTarget!=null)
        {
            FightUtility.MoveTowardsTarget(gameObject,mTarget.gameObject,mMoveSpeed,OffsetAngle);
        }
    }
}