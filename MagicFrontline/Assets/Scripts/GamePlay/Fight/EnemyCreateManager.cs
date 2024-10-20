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
    private EnemyWaveUI mEnemyWaveUI;
    private int mCurrentWaveIndex=0;
    //当前波次敌人生成是否完成
    private bool mIsEnemyWaveCreate=true;
    public EnemyCreateManager()
    {   
        mEnemies=FightModel.GetCurrent().GetEnemies();
        mMap=FightModel.GetCurrent().GetMap();
        mPendingEnemy=new List<List<EnemyId>>();
        LevelOneEnemyListInit();
        mEnemyWaveUI=EnemyWaveUI.Create(mPendingEnemy.Count);
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
        AddEnemyIdList(EnemyId.Enemy1,20);
        AddEnemyIdList(EnemyId.Enemy1,30);
        AddEnemyIdList(EnemyId.Enemy1,40);
        AddEnemyIdList(EnemyId.Enemy1,40);
        AddEnemyIdList(EnemyId.Enemy1,40);
        //AddEnemyIdList(EnemyId.Enemy2,10);
        //AddEnemyIdList(EnemyId.Enemy3,10);
        //AddEnemyIdList(EnemyId.Enemy4,10);
    }

    public void CreateEnemyWave()
    {
        mEnemyWaveUI.OnEnemyWavesChanged();
        int count=mPendingEnemy[mCurrentWaveIndex].Count;
        for(int i=0;i<count;i++)
        {
            //保存当前变量供延迟函数用
            int index=i;
            int level=mLevel;
            DOVirtual.DelayedCall(i*mEnemyCreateTime,()=>
            { 
                Vector3Int logicPosition=mMap.GetTurningPointList()[0];
                Vector3 worldPosition=mMap.LogicToWorldPosition(logicPosition);
                EnemyId enemyId=mPendingEnemy[mCurrentWaveIndex][index];
                Enemy Enemy=NormalEnemy.Create(enemyId,new Vector3(worldPosition.x,worldPosition.y,-1),level);
                mEnemies.Add(Enemy);
                if(index==count-1)
                {
                    mIsEnemyWaveCreate=true;
                }
            },false);
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
                Debug.Log(mCurrentWaveIndex);
            });
            
        }
    }
}