using UnityEngine;

//喷石嘴子弹
public class BulletJet : Bullet
{
    public static BulletJet Create(Tower tower,Enemy target,int points)
    {
        GameObject bulletPrefab=Resources.Load<GameObject>("FightObject/Bullet/BulletJet");
        GameObject bulletObject=GameObject.Instantiate(bulletPrefab);
        BulletJet bullet=bulletObject.AddComponent<BulletJet>();
        bullet.Init(tower,target,points);
        return bullet;
    }
    protected void Init(Tower tower,Enemy target,int points)
    {
        base.Init(tower,target,points);
        mAnimator.Play("BulletJet"+tower.GetLevel().ToString());
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        FightUtility.MoveTowardsRotation(gameObject,mMoveSpeed,OffsetAngle);
    }
}