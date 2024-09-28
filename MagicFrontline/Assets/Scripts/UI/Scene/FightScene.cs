using UnityEngine;

public class FightScene : MonoBehaviour
{
    FightManager mFightManager;
    public void Awake()
    {
       InitScene();
       mFightManager=new FightManager();
    }
    public void Update()
    {
        mFightManager.OnUpdate();
    }
    public void Start()
    {
         
    }
    public void InitScene()
    {
        Camera camera=Camera.main;
        camera.orthographicSize=Utility.WindowHeight/2;
    }

}