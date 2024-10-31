using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    public void Awake()
    {
        AudioManager.AudioManagerInit(GameObject.Find("AudioManager").AddComponent<AudioManager>());
        GameManager.GetInstance();
    }

    public void Start()
    {
        SceneManager.LoadScene("Main");
    }
}