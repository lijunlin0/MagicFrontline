using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    private float OffsetAngle=-90;
    private const float mPauseTime=2f;
    private const int MoveSpeedReductionFrame=30;
    private Vector3 mTargetPosition;
    private float mCreateTime=0;
    private int mPoints;
    //protected AudioSource mSound;

    private float mMoveSpeed;
    
    public static Coin Create(Vector3 createPosition, int points)
    {
        GameObject prefab=Resources.Load<GameObject>("FightObject/Other/Coin");
        GameObject coinObject=GameObject.Instantiate(prefab);
        coinObject.transform.position=createPosition;
        Coin Coin = coinObject.AddComponent<Coin>();
        Coin.Init(points);
        return Coin;
    }

    private void Init(int points)
    {
        mPoints=points;
        mTargetPosition=FightManager.GetCurrent().GetCointUI().transform.position;
        //mSound=GetComponent<AudioSource>();
        mMoveSpeed=500;
    }

    public void Update()
    {
        mCreateTime+=Time.deltaTime;
        if(Vector3.Distance(mTargetPosition,transform.position)<=50)
        {
            FightModel.GetCurrent().AddCoins(mPoints);
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