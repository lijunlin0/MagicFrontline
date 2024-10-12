public class Property
{
    private int mMaxHealth;
    private int mHealthAdd;
    private int mCurrentHealth;
    private int mBaseMoveSpeed;
    private int mCurrentMoveSpeed;
    public void Init(int baseHealth,int healthAdd,int baseMoveSpeed)
    {
        mMaxHealth=baseHealth;
        mHealthAdd=healthAdd;
        mBaseMoveSpeed=baseMoveSpeed;
        mCurrentHealth=mMaxHealth;
        mCurrentMoveSpeed=mBaseMoveSpeed;
    }

    public int GetBaseHealth(){return mMaxHealth;}
    public int GetCurrentHealth(){return mCurrentHealth;}
    public int GetBaseMoveSpeed(){return mBaseMoveSpeed;}
    public int GetCurrentMoveSpeed(){return mCurrentMoveSpeed;}
    public int GetHealthAdd(){return mHealthAdd;}

    public void ChangeCurrentHealth(int changeHealth){ mCurrentHealth+=changeHealth;}
    public void ChangeCurrentMoveSpeed(int changeMoveSpeed){mCurrentMoveSpeed+=changeMoveSpeed;}
}