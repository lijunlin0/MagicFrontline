using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCreateManager
{
    private int mLevel=1;
    private Map mMap;
    private float mEnemyCreateDefaultTime=0;
    private float mEnemyCreateTime=1f;
    private Vector3Int mCreatePosition;
    private List<Enemy> mEnemies;
    private List<List<EnemyId>> mPendingEnemy;
    private int mCurrentWaveIndex=0;
    //当前波次敌人生成是否完成
    private bool mIsEnemyWaveCreate=true;
    public EnemyCreateManager()
    {   
        mEnemies=FightModel.GetCurrent().GetEnemies();
        mMap=FightModel.GetCurrent().GetMap();
        mPendingEnemy=new List<List<EnemyId>>();
        LevelEnemyListInit();
    }

    public int GetMaxWave(){return mPendingEnemy.Count;}

    private void AddEnemyIdList(EnemyId enemyId,int num)
    {
        List<EnemyId> idList=new List<EnemyId>();
        for(int i=0;i<num;i++)
        {
            idList.Add(enemyId);
        }
        mPendingEnemy.Add(idList);
    }

    public  void LevelEnemyListInit()
    {
        mPendingEnemy.Clear();
        List<Tuple<EnemyId,int>> enemyList=LevelEnemyCreateUtility.GetBaseEnemyCreateList(FightModel.GetCurrent().GetLevel());
        foreach(Tuple<EnemyId,int> pair in enemyList)
        {
            AddEnemyIdList(pair.Item1,pair.Item2);
        }
    }

    public void CreateEnemyWave()
    {
        FightManager.GetCurrent().GetEnemyWaveUI().OnEnemyWavesChanged();
        int count=mPendingEnemy[mCurrentWaveIndex].Count;
        for(int i=0;i<count;i++)
        {
            //保存当前变量供延迟函数用
            int index=i;
            int currentWaveIndex=mCurrentWaveIndex;
            int level=mLevel;
            DOVirtual.DelayedCall(i*mEnemyCreateTime,()=>
            { 
                Vector3Int logicPosition=mMap.GetTurningPointList()[0];
                Vector3 worldPosition=mMap.LogicToWorldPosition(logicPosition);
                EnemyId enemyId=mPendingEnemy[currentWaveIndex][index];
                Enemy Enemy=NormalEnemy.Create(enemyId,new Vector3(worldPosition.x,worldPosition.y,-count+index-1),level);
                mEnemies.Add(Enemy);
                if(index==count-1)
                {
                    mIsEnemyWaveCreate=true;
                }
            },false).SetId("EnemyCreate");
        }
        mCurrentWaveIndex++;
        mLevel++;
    }
    public void OnUpdate()
    {
        //场上还有敌人或当前这波敌人没生成完成则不生成下一波
        if(mEnemies.Count!=0||!mIsEnemyWaveCreate)
        {
            return;
        }
        if(mCurrentWaveIndex<mPendingEnemy.Count)
        {
            mIsEnemyWaveCreate=false;
            DOVirtual.DelayedCall(3,()=>
            {
                CreateEnemyWave();
            },false);
        }
        else
        {
            FightManager.GetCurrent().End(true);
        }
    }
}