public class Property
{
    private int mBaseHealth;
    private int mHealthAdd;
    private int mCurrentHealth;
    private int mBaseMoveSpeed;
    private int mCurrentMoveSpeed;
    public void Init(int baseHealth,int healthAdd,int baseMoveSpeed)
    {
        mBaseHealth=baseHealth;
        mHealthAdd=healthAdd;
        mBaseMoveSpeed=baseMoveSpeed;
        mCurrentHealth=mBaseHealth;
        mCurrentMoveSpeed=mBaseMoveSpeed;
    }

    public int GetBaseHealth(){return mBaseHealth;}
    public int GetCurrentHealth(){return mCurrentHealth;}
    public int GetBaseMoveSpeed(){return mBaseMoveSpeed;}
    public int GetCurrentMoveSpeed(){return mCurrentMoveSpeed;}
    public int GetHealthAdd(){return mHealthAdd;}

    public void SetCurrentHealth(int currentHealth){ mCurrentHealth=currentHealth;}
    public void SetCurrentMoveSpeed(int currentMoveSpeed){mCurrentMoveSpeed=currentMoveSpeed;}
}