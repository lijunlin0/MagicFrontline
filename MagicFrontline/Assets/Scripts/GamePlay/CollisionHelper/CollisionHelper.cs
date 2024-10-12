using System.Collections.Generic;
using DG.Tweening;

public static class CollisionHelper
{
    public static bool IsColliding(MyCollider collider1,MyCollider collider2)
    {
        if(!collider1.IsEnable()||!collider2.IsEnable())
        {
            return false;
        }
        return collider1.GetCollider().Distance(collider2.GetCollider()).isOverlapped;
    }

    //子弹碰撞敌人
    public static void Collide()
    {
        List<Bullet> bullets=FightModel.GetCurrent().GetBullets();
        List<Enemy> enemies=FightModel.GetCurrent().GetEnemies();
        foreach(Bullet bullet in bullets)
        {
            MyCollider collider1=bullet.GetCollider();
            foreach(Enemy enemy in enemies)
            {
                MyCollider collider2=enemy.GetCollider();
                if(IsColliding(collider1,collider2))
                {
                    bullet.OnColliderEnemy(enemy);
                }
            }
        }
    }
}