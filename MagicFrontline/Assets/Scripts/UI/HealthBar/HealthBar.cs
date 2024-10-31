using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const int YOffset=35;
    private const int BossYOFfset=70;
    private int mYOffset;
    private Enemy mEnemy;
    private Slider mHealthSlider;
    private Slider mDamageSlider;
    private Tween mTween;
    public static HealthBar Create(Enemy enemy)
    {
        GameObject prefab=Resources.Load<GameObject>("UI/HealthBar");
        GameObject healthObject=Instantiate(prefab,GameObject.Find("BarCanvas").transform);
        int yOffset;
        if(enemy.IsBoss())
        {
            yOffset=BossYOFfset;
            healthObject.transform.localScale=new Vector3(healthObject.transform.localScale.x*3,healthObject.transform.localScale.y*2,1);
        }
        else
        {
            yOffset=YOffset;
        }
        
        HealthBar healthBar=healthObject.AddComponent<HealthBar>();
        healthBar.Init(enemy,yOffset);
        return healthBar;

    }

    private void Init(Enemy enemy,int yOffset)
    {
        mEnemy=enemy;
        mYOffset=yOffset;
        Vector3 enemyTopPosition=enemy.GetTopPosition();
        transform.position=new Vector3(enemyTopPosition.x,enemyTopPosition.y+mYOffset,enemyTopPosition.z);
        mHealthSlider=transform.Find("Health").GetComponent<Slider>();
        mDamageSlider=transform.Find("Damage").GetComponent<Slider>();
        mHealthSlider.value=1;
        mDamageSlider.value=1;
    }

    public void OnHealthChanged()
    {
        float maxHealth=mEnemy.GetProperty().GetBaseHealth();
        float currentHealth=mEnemy.GetHealth();
        float targetValue=currentHealth/maxHealth;
        mHealthSlider.value=targetValue;
        //如果有旧动画时,终止其他动画 ---在短时间收到多次伤害时会发生
        if(mTween!=null)
        {
            mTween.Kill();
        }
        mTween=mDamageSlider.DOValue(targetValue,0.2f).SetEase(Ease.InQuint).OnComplete(()=>
        {
            mTween=null;
        });
    }

    public void Update()
    {
        if(mEnemy.IsDead())
        {
            mHealthSlider.value=0;
            mTween.Kill();
            Destroy(gameObject);
        }
        Vector3 enemyPosition=mEnemy.GetTopPosition();
        transform.position=new Vector3(enemyPosition.x,enemyPosition.y+mYOffset,0);
    }
}