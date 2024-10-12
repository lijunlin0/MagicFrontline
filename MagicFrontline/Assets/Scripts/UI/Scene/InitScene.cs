using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    public void Awake()
    {
        GameManager.GetInstance();
    }

    public void Start()
    {
        SceneManager.LoadScene("Main");
    }
}