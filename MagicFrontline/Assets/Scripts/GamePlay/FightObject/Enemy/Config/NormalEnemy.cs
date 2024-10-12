using UnityEngine;

public class NormalEnemy :Enemy
{
    public static NormalEnemy Create(EnemyId enemyId,Vector3 position,int level)
    {
        GameObject enemyPrefab=Resources.Load<GameObject>("FightObject/Enemy/"+enemyId.ToString());
        GameObject enemyObject=Instantiate(enemyPrefab);
        enemyObject.transform.position=position;
        NormalEnemy enemy = enemyObject.AddComponent<NormalEnemy>();
        Property property=EnemyUtility.GetBasePropertySheet(enemyId.ToString(),level);
        enemy.Init(EnemyId.Enemy1,property);
        return enemy;
    }
}