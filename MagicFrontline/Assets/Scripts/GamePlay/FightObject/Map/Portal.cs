using UnityEngine;

public class Portal:MonoBehaviour
{
    public static void Create(Vector3Int createPosition,bool isHome)
    {
        GameObject prefab=Resources.Load<GameObject>("FightObject/Other/Portal");
        GameObject protalObject=Instantiate(prefab);
        Portal portal=protalObject.AddComponent<Portal>();
        portal.Init(createPosition,isHome);
    }
    public void Init(Vector3Int createPosition,bool isHome)
    {
        transform.position=FightModel.GetCurrent().GetMap().LogicToWorldPosition(createPosition);
        transform.position=new Vector3(transform.position.x,transform.position.y,-100);
        if(!isHome)
        {
            SpriteRenderer spriteRenderer=transform.GetComponent<SpriteRenderer>();
            spriteRenderer.color=Color.red;
        }
        
    }
}