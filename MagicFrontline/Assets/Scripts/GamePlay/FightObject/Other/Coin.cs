using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    private const float OffsetPosition=100;
    private float OffsetAngle=-90;
    private const float PauseTime=2f;
    private const float OffsetPauseTime=0.5f;
    private float mPauseTime;
    private const int MoveSpeedReductionFrame=30;
    private const int mPoints=20;
    private Vector3 mTargetPosition;
    private float mCreateTime=0;

    //protected AudioSource mSound;

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
        mTargetPosition=FightManager.GetCurrent().GetCointUI().transform.position;
        //mSound=GetComponent<AudioSource>();
        mPauseTime=PauseTime+Random.Range(-OffsetPauseTime,OffsetPauseTime);
        mMoveSpeed=500;
    }

    public void Update()
    {
        mCreateTime+=Time.deltaTime;
        if(Vector3.Distance(mTargetPosition,transform.position)<=50)
        {
            FightModel.GetCurrent().AddCoins(mPoints);
            FightManager.GetCurrent().GetCointUI().PlayCSound();
            //mSound.Play();
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