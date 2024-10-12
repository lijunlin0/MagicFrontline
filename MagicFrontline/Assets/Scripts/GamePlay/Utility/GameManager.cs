using DG.Tweening;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //构造
    public void OnSingletonInit()
    {
        Application.targetFrameRate=60;
        DOTween.Init();
        Utility.Init();
        EnemyUtility.Init();
        TowerUtility.Init();
    }
}