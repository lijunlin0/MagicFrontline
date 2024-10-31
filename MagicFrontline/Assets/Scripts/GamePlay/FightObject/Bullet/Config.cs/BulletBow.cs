using UnityEngine;

//弓箭子弹 -- 跟踪子弹
public class BulletBow : Bullet
{
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
        base.Init(tower,target,points);
        mAnimator.Play("BulletBow"+tower.GetLevel().ToString());
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(mTarget!=null)
        {
            FightUtility.MoveTowardsTarget(gameObject,mTarget,mMoveSpeed,OffsetAngle);
        }
    }
}