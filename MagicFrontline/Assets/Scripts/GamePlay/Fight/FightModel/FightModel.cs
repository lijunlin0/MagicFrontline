
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FightModel
{
    private static FightModel sCurrent;
    private int mLevel;
    private int mCoins;
    private int mHealth;
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
    public int GetEnemyMaxWave(){return mEnemyCreateManager.GetMaxWave();}
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
        FightManager.GetCurrent().GetCoinUI().OnCoinsChanged();
    }
    public void RemoveCoins(int number)
    {
        mCoins-=number;
        FightManager.GetCurrent().GetCoinUI().OnCoinsChanged();
    }
    public void SetLevel(int level){mLevel=level;}
    public int GetLevel(){return mLevel;}
    public void SetHealth(int health){mHealth=health;}
    public void SetCoins(int coins){mCoins=coins;}
    public void GetHurt(int points)
    {
        mHealth = Mathf.Max(mHealth - points, 0);
        if(mHealth==0)
        {
            FightManager.GetCurrent().End(false);
        }
        FightManager.GetCurrent().GetHealthUI().OnHealthsChanged();
    }
    public int GetHealth(){return mHealth;}
    public static int LevelNameToLevel(string levelName)
    {
        return int.Parse(levelName.Substring(5));
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
        mMap=Map.Create();
        Map.CreatePortal();
        mLevel=LevelNameToLevel(SceneManager.GetActiveScene().name);
        mEnemyCreateManager=new EnemyCreateManager();
        mTowerCreateManager=new TowerCreateManager();
        AudioManager.GetCurrent().PlayFightMusic(mLevel);
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