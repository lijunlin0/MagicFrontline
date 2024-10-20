using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const int YOffset=100;
    private Enemy mEnemy;
    private Slider mHealthSlider;
    private Slider mDamageSlider;
    private Tween mTween;
    public static HealthBar Create(Enemy enemy)
    {
        GameObject prefab=Resources.Load<GameObject>("UI/HealthBar");
        GameObject healthObject=Instantiate(prefab,GameObject.Find("Canvas").transform);
        healthObject.transform.position=new Vector3(enemy.transform.position.x,enemy.transform.position.y+YOffset,enemy.transform.position.z);
        HealthBar healthBar=healthObject.AddComponent<HealthBar>();
        healthBar.Init(enemy);
        return healthBar;

    }

    private void Init(Enemy enemy)
    {
        mEnemy=enemy;
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
        transform.position=mEnemy.transform.position+new Vector3(0,YOffset,0);
    }
}