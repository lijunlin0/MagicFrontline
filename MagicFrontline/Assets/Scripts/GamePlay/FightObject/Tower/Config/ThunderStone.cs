using System;
using System.Collections.Generic;
using UnityEngine;

//电击宝石
public class ThunderStone : Tower
{
    protected int mBulletCount=3;
    public static ThunderStone Create(int level,Vector3Int position,Quaternion rotation = default)
    {
        GameObject towerPrefab=Resources.Load<GameObject>("FightObject/Tower/ThunderStone/ThunderStone");
        GameObject towerObject=Instantiate(towerPrefab);
        towerObject.transform.rotation=rotation;
        ThunderStone thunderStone=towerObject.AddComponent<ThunderStone>();
        var property=TowerUtility.GetBasePropertySheet("ThunderStone",level);
        thunderStone.Init(level,position,property);
        return thunderStone;
    }

    public void Init(int level,Vector3Int position,Tuple<int,float,int> property)
    {
        base.Init(level,position,property,"ThunderStone");
        mIsRotate=false;
        mBulletCount+=mLevel-1;
        Debug.Log("电击个数:"+mBulletCount);
        mShootCallback=()=>
        {
            List<Enemy> targetEnemies=new List<Enemy>(); 
            List<GameObject> gameObjects=new List<GameObject>();
            for(int i=0;i<mBulletCount;i++)
            {
                Enemy targetEnemy=FightUtility.GetTargetEnemy(transform.position,mShootRange,StatusEffectId.None,targetEnemies);
                if(targetEnemy==null)
                {
                    break;
                }
                targetEnemies.Add(targetEnemy);
                gameObjects.Add(targetEnemy.gameObject);
                targetEnemy.Damage(mAttack);
            }
            if(targetEnemies.Count!=0)
            {
                gameObjects.Insert(0,this.gameObject);
                FightUtility.ChainEffect(this,gameObjects);
            }
        };
    }
}
