using UnityEngine;

//闪电宝石子弹
public class BulletThunderStone : Bullet
{

    public static BulletThunderStone Create(Tower tower)
    {
        GameObject prefab = Resources.Load<GameObject>("FightObject/Bullet/BulletThunderStone");
        GameObject bulletObject=GameObject.Instantiate(prefab);
        BulletThunderStone bullet=bulletObject.AddComponent<BulletThunderStone>();
        bullet.Init(tower);
        return bullet;
    }

    private void Init(Tower tower)
    {
        mSource=tower;
        mAnimator=GetComponent<Animator>();
        mPoints=0;
        mLiveTime=0;
        mMaxLifeTime=5;
        mIsPenetrate=false;    
        mAnimator.Play("Bullet"+tower.GetName()+mSource.GetLevel());
    }
    
}