using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class FightUtility
{
    public static Vector3 RotationToDirection(Quaternion rotation,float offsetAngle=0)
    {
        // 返回方向向量
        return rotation * Vector3.up; // 对于2D游戏，使用 Vector3.up
    }
    public static Quaternion DirectionToRotation(Vector3 direction,float offsetAngle=0f)
    {
        float angle=DirectionToRadian(direction)*Mathf.Rad2Deg;
        return Quaternion.Euler(0,0,angle+offsetAngle);
    }
    public static float DirectionToRadian(Vector3 direction)
    {
        return Mathf.Atan2(direction.y,direction.x);
    }
    public static void MoveTowardsRotation(GameObject gameObject,float MoveSpeed,float offsetAngle=0)
    {
        Vector3 moveDirection=FightUtility.RotationToDirection(gameObject.transform.rotation,offsetAngle);
        Debug.Log("移动:"+moveDirection+",速度"+MoveSpeed);
        gameObject.transform.position+=moveDirection*MoveSpeed*Time.deltaTime;
    }
    public static void MoveTowardsTarget(GameObject gameObject,GameObject target,int moveSpeed,float offsetAngle=0)
    {
        //朝向目标
        Vector3 direction=(target.transform.position-gameObject.transform.position).normalized;
        gameObject.transform.rotation=FightUtility.DirectionToRotation(direction,offsetAngle);
        //向目标移动
        Vector3 moveDirection=(target.transform.position-gameObject.transform.position).normalized;
        gameObject.transform.position+=moveDirection*moveSpeed*Time.deltaTime;
    }

    public static Enemy GetTargetEnemy(Vector3 position,int range,StatusEffectId statusEffectId=StatusEffectId.None,List<Enemy> ignoreList=null)
    {
        List<Enemy> enemies=FightModel.GetCurrent().GetEnemies();
        List<Enemy> res=new List<Enemy>();
        //获取在射程内的敌人
        foreach(Enemy enemy in enemies)
        {
            if(ignoreList!=null&&ignoreList.Contains(enemy))
            {
                continue;
            }
            float distance=Vector3.Distance(position,enemy.transform.position);
            if(distance<range)
            {
                res.Add(enemy);
            }
        }
        float maxProgress=-1;
        float maxEffectProgress=-1;
        Enemy fastEnemy=null;
        Enemy fastEffectEnemy=null;
        foreach(Enemy enemy in res)
        {
            if(ignoreList!=null&&ignoreList.Contains(enemy))
            {
                continue;
            }
            float progress=enemy.GetMoveProgress();
            //没有状态效果筛选,获取走在最前面的敌人
            if(statusEffectId==StatusEffectId.None)
            {
                if(progress>maxProgress)
                {
                    fastEnemy=enemy;
                    maxProgress=progress;
                }
            }
            //有状态效果筛选
            else
            {
                //筛选出没有持有该状态效果的走在最前面的敌人
                if(!enemy.HasStatusEffect(statusEffectId))
                {
                    if(progress>maxProgress)
                    {
                        fastEnemy=enemy;
                        maxProgress=progress;
                    }
                }
                //都持有该状态效果时,获取走在最前面的敌人
                else
                {
                    if(progress>maxEffectProgress)
                    {
                        fastEffectEnemy=enemy;
                        maxEffectProgress=progress;
                    }
                }
            }
            
        }
        return fastEnemy!=null?fastEnemy:fastEffectEnemy;
    }
    public static void ChainEffect(Tower tower,List<GameObject> enemies)
    {
        
        for(int i=1;i<enemies.Count;i++)
        {
            GameObject startEnemy=enemies[i-1];
            GameObject endEnemy=enemies[i];
            BulletThunderStone effect=BulletThunderStone.Create(tower);
            Callback updatePosition=()=>
            {
                LineLink(effect.gameObject,startEnemy.transform.position,endEnemy.transform.position);
            };
            updatePosition();
            DOVirtual.Float(0,1,0.5f,(float f)=>
            {
                updatePosition();
            }).OnComplete(()=>{
                GameObject.Destroy(effect.gameObject);
            });
        }
    }
    public static void LineLink(GameObject effect,Vector3 startPosition,Vector3 endPosition,float xFactor=0.005f)
    {
        Vector3 direction=endPosition-startPosition;
        effect.transform.position=(startPosition+endPosition)/2;
        effect.transform.position=new Vector3(effect.transform.position.x,effect.transform.position.y,-1);
        effect.transform.rotation=DirectionToRotation(direction);
        float distance=MathF.Sqrt(SqrDistance2D(startPosition,endPosition));
        float scaleY=effect.transform.localScale.y;
        effect.transform.localScale=new Vector3(distance*xFactor,scaleY,1);
    }
    public static float SqrDistance2D(Vector3 position1,Vector3 position2)
    {
        float dx=position1.x-position2.x;
        float dy=position1.y-position2.y;
        return dx*dx+dy*dy;
    }

}