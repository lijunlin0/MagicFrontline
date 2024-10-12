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
    public EnemyCreateManager()
    {       
        mEnemies=FightModel.GetCurrent().GetEnemies();
        mMap=FightModel.GetCurrent().GetMap();
        mPendingEnemy=new List<List<EnemyId>>();
        LevelOneEnemyListInit();
    }

    private void AddEnemyIdList(EnemyId enemyId,int num)
    {
        List<EnemyId> idList=new List<EnemyId>();
        for(int i=0;i<num;i++)
        {
            idList.Add(enemyId);
        }
        mPendingEnemy.Add(idList);
    }

    public  void LevelOneEnemyListInit()
    {
        mPendingEnemy.Clear();
        AddEnemyIdList(EnemyId.Enemy1,10);
        //AddEnemyIdList(EnemyId.Enemy2,10);
        //AddEnemyIdList(EnemyId.Enemy3,10);
        //AddEnemyIdList(EnemyId.Enemy4,10);
    }

    public void CreateEnemyWave()
    {
        for(int i=0;i<mPendingEnemy[mCurrentWaveIndex].Count;i++)
        {
            EnemyId enemyId=mPendingEnemy[mCurrentWaveIndex][i];
            DOVirtual.DelayedCall(i*mEnemyCreateTime,()=>
            {  
                Vector3Int logicPosition=mMap.GeturningPointList()[0];
                Vector3 worldPosition=mMap.LogicToWorldPosition(logicPosition);
                Enemy Enemy=NormalEnemy.Create(enemyId,new Vector3(worldPosition.x,worldPosition.y,-1),mLevel);
                mEnemies.Add(Enemy);
            },false);
        }
        mCurrentWaveIndex++;
    }
    public void OnUpdate()
    {
        if(mEnemies.Count!=0)
        {
            return;
        }
        if(mCurrentWaveIndex<mPendingEnemy.Count)
        {
            CreateEnemyWave();
            Debug.Log(mCurrentWaveIndex);
        }
    }
}