using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager
{
    private static bool mPause;
    private static FightManager sCurrent;
    private CoinUI mCoinUI;
    protected FightModel mFightModel;
    
    public static FightManager GetCurrent()
    {
        return sCurrent;
    }
    public FightManager()
    {
        sCurrent=this;
        mFightModel=new FightModel();
        mCoinUI=CoinUI.Create();
    }
    public void OnUpdate()
    {
        mFightModel.OnUpdate();
    }
    public void SetPause(bool pause)
    {
        mPause=pause;
    }
    public static bool IsPause(){return mPause;}

    public CoinUI GetCointUI(){return mCoinUI;}
}
