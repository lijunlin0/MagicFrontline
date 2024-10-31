using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    private const float OffsetPosition=40;
    private float OffsetAngle=-90;
    private const float PauseTime=1;
    private const float OffsetPauseTime=0.5f;
    private float mPauseTime;
    private const int MoveSpeedReductionFrame=30;
    private const int mPoints=40;
    private Vector3 mTargetPosition;
    private float mCreateTime=0;

    private float mMoveSpeed;
    
    public static void Create(Vector3 createPosition,int createCount)
    {
        GameObject prefab=Resources.Load<GameObject>("FightObject/Other/Coin");
        
        for(int i=0;i<createCount;i++)
        {
            GameObject coinObject=GameObject.Instantiate(prefab);
            float x= Random.Range(-OffsetPosition,OffsetPosition);
            float y= Random.Range(-OffsetPosition,OffsetPosition);
            coinObject.transform.position=createPosition+new Vector3(x,y,0);
            Coin coin = coinObject.AddComponent<Coin>();
            coin.Init(createPosition);
        }
    }

    private void Init(Vector3 createPosition)
    {
        mTargetPosition=FightManager.GetCurrent().GetCoinUI().transform.position;
        mPauseTime=PauseTime+Random.Range(-OffsetPauseTime,OffsetPauseTime);
        mMoveSpeed=500;
    }

    public void Update()
    {
        mCreateTime+=Time.deltaTime;
        if(Vector3.Distance(mTargetPosition,transform.position)<=50)
        {
            FightModel.GetCurrent().AddCoins(mPoints);
            AudioManager.GetCurrent().PlayCoinSound();
            Destroy(gameObject);
            return;
        }
        if(mCreateTime<=mPauseTime)
        {
            return;
        }
        Vector3 direction=(mTargetPosition-transform.position).normalized;
        transform.localRotation=FightUtility.DirectionToRotation(direction,OffsetAngle);
        mMoveSpeed+=MoveSpeedReductionFrame;
        FightUtility.MoveTowardsRotation(gameObject,mMoveSpeed,OffsetAngle);
    }


}