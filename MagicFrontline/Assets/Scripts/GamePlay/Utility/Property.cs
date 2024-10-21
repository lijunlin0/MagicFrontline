public class Property
{
    private int mMaxHealth;
    private int mCurrentHealth;
    private float mBaseMoveSpeed;
    private float mCurrentMoveSpeed;
    private int mBaseCoinCount;
    public void Init(int baseHealth,int baseMoveSpeed,int baseCoinCount)
    {
        mMaxHealth=baseHealth;
        mBaseMoveSpeed=baseMoveSpeed;
        mCurrentHealth=mMaxHealth;
        mCurrentMoveSpeed=mBaseMoveSpeed;
        mBaseCoinCount=baseCoinCount;
    }

    public int GetBaseHealth(){return mMaxHealth;}
    public int GetCurrentHealth(){return mCurrentHealth;}
    public float GetBaseMoveSpeed(){return mBaseMoveSpeed;}
    public float GetCurrentMoveSpeed(){return mCurrentMoveSpeed;}
    public int GetBaseCoinCount(){return mBaseCoinCount;}

    public void ChangeCurrentHealth(int changeHealth){ mCurrentHealth+=changeHealth;}
    public void ChangeCurrentMoveSpeed(int changeMoveSpeed){mCurrentMoveSpeed+=changeMoveSpeed;}
}