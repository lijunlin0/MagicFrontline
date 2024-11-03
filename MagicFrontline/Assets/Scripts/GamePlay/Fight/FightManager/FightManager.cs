using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager
{
    private static bool mPause;
    private static FightManager sCurrent;
    private CoinUI mCoinUI;
    private EnemyWaveUI mEnemyWaveUI;
    private HealthUI mHealthUI;
    protected FightModel mFightModel;
    
    public static FightManager GetCurrent()
    {
        return sCurrent;
    }
    public FightManager()
    {
        mPause=false;
        sCurrent=this;
        mFightModel=new FightModel();
        mCoinUI=CoinUI.Create();
        mEnemyWaveUI=EnemyWaveUI.Create();
        mHealthUI=HealthUI.Create();
        
    }
    public void OnUpdate()
    {
        if(mPause)
        {
            return;
        }
        mFightModel.OnUpdate();
    }
    public void SetPause(bool pause)
    {
        mPause=pause;
        if(mPause)
        {
            DOTween.Pause("EnemyCreate");
        }
        else
        {

            DOTween.Play("EnemyCreate");
        }
    }
    public static bool IsPause(){return mPause;}

    public CoinUI GetCoinUI(){return mCoinUI;}
    public HealthUI GetHealthUI(){return mHealthUI;}
    public EnemyWaveUI GetEnemyWaveUI(){return mEnemyWaveUI;}
    public void End(bool isWin)
    {
        if(isWin)
        {
            LevelUtility.SetLevelPass(FightModel.GetCurrent().GetLevel());
        }
        EndWindow.Create(isWin);
    }
}
