
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FightModel
{
    private static FightModel sCurrent;
    private int mCoins;
    private List<Enemy> mEnemyList;
    private Dictionary<int,Tower> mTowers;
    private List<Bullet> mBulletList;
    private Map mMap;
    private EnemyCreateManager mEnemyCreateManager;
    private TowerCreateManager mTowerCreateManager;
    public Map GetMap(){return mMap;}
    public List<Enemy> GetEnemies(){return mEnemyList;}
    public Dictionary<int,Tower> GetTowers(){return mTowers;}
    public List<Bullet> GetBullets(){return mBulletList;}
    public TowerCreateManager GetTowerCreateManager(){return mTowerCreateManager;}
    public void AddEnemy(Enemy enemy){mEnemyList.Add(enemy);}
    public void AddTower(Tower tower,Vector3Int position)
    {
        mMap.AddTower(position);
        int key=mMap.LogicPositionToInt(position);
        mTowers[key]=tower;
    }
    public void AddCoins(int number)
    {
        mCoins+=number;
        FightManager.GetCurrent().GetCointUI().OnCoinsChanged(number);
    }
    public void RemoveCoins(int number)
    {
        mCoins-=number;
        FightManager.GetCurrent().GetCointUI().OnCoinsChanged(-number);
    }
    public int GetCoins(){return mCoins;}
    public void RemoveTower(Vector3Int position)
    {
        int key=mMap.LogicPositionToInt(position);
        Tower tower=mTowers[key];
        mTowers.Remove(key);
        mMap.RemoveTower(position);
        tower.PlayDestroyAnimation();
    }
    public Tower GetTower(Vector3Int position)
    {
        int key=mMap.LogicPositionToInt(position);
        return mTowers[key];
    }

    public void AddBullet(Bullet bullet){mBulletList.Add(bullet);}
    public static FightModel GetCurrent()
    {
        return sCurrent;
    }

    public FightModel()
    {
        sCurrent=this;
        mEnemyList=new List<Enemy>();
        mTowers=new Dictionary<int,Tower>();
        mBulletList=new List<Bullet>();
        mCoins=1000;
        mMap=Map.Create();
        Map.CreatePortal();
        mEnemyCreateManager=new EnemyCreateManager();
        mTowerCreateManager=new TowerCreateManager();
    }

    public void OnUpdate()
    {
        mEnemyCreateManager.OnUpdate();
        mTowerCreateManager.OnUpdate();
        CollisionHelper.Collide();
        UpdateObjects();
        RemoveInvalidObjects();
    }
    private void UpdateObjects()
    {
        foreach(Enemy enemy in mEnemyList)
        {
            enemy.OnUpdate();
        }
        foreach(Tower tower in mTowers.Values)
        {
            tower.OnUpdate();
        }
        foreach(Bullet bullet in mBulletList)
        {
            bullet.OnUpdate();
        }
    }
    private void RemoveInvalidObjects()
    {
        //删除销毁的子弹
        for(int i=0;i<mBulletList.Count;)
        {
            Bullet bullet=mBulletList[i];
            if(bullet.IsDead())
            {
                mBulletList.RemoveAt(i);
                bullet.PlayDestroyAnimation();
            }
            else
            {
                i++;
            }
        }
        //删除销毁的敌人
        for(int i=0;i<mEnemyList.Count;)
        {
            Enemy enemy=mEnemyList[i];
            if(enemy.IsDead())
            {
                mEnemyList.RemoveAt(i);
                enemy.PlayDestroyAnimation();
            }
            else
            {
                i++;
            }
        }
        
    } 
}