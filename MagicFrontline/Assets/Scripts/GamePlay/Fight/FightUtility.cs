using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightUtility
{
    public static Vector3 RotationToDirection(Quaternion rotation)
    {
        float radian=rotation.eulerAngles.z*Mathf.Deg2Rad;
        return new Vector3(MathF.Cos(radian),Mathf.Sin(radian),0).normalized;
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
    public static void MoveTowardsRotation(GameObject gameObject,int MoveSpeed)
    {
        Vector3 moveDirection=FightUtility.RotationToDirection(gameObject.transform.rotation);
        gameObject.transform.position+=moveDirection*MoveSpeed*Time.deltaTime;
    }
    public static void MoveTowardsTarget(GameObject gameObject,GameObject target,int moveSpeed,float offsetAngle)
    {
        //朝向目标
        Vector3 direction=(target.transform.position-gameObject.transform.position).normalized;
        gameObject.transform.rotation=FightUtility.DirectionToRotation(direction,offsetAngle);
        //向目标移动
        Vector3 moveDirection=(target.transform.position-gameObject.transform.position).normalized;
        gameObject.transform.position+=moveDirection*moveSpeed*Time.deltaTime;
    }

    public static Enemy GetTargetEnemy(Tower tower,int range)
    {
        List<Enemy> enemies=FightModel.GetCurrent().GetEnemies();
        List<Enemy> res=new List<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            float distance=Vector3.Distance(tower.transform.position,enemy.transform.position);
            if(distance<range)
            {
                Debug.Log("distance:"+distance);
                res.Add(enemy);
            }
        }
        float maxProgress=-1;
        Enemy fastEnemy=null;
        foreach(Enemy enemy in res)
        {
            float progress=enemy.GetMoveProgress();
            if(progress>maxProgress)
            {
                fastEnemy=enemy;
                maxProgress=progress;
            }
        }
        return fastEnemy;
    }

}