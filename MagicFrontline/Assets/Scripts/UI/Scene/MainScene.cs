using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene: MonoBehaviour
{
    public void Awake()
    {
        GameObject canvas=GameObject.Find("Canvas");
        Camera mainCamera = Camera.main;
        RectTransform transform=canvas.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(Utility.WindowWidth, Utility.WindowHeight); 
        Debug.Log(Utility.WindowHeight/2);
        mainCamera.orthographicSize=Utility.WindowHeight/2;
        Button button=transform.Find("Button").gameObject.GetComponent<Button>();
        button.onClick.AddListener(StartButtonOnClick);
    }
    public void StartButtonOnClick()
    {
        SceneManager.LoadScene("Fight");
    }
}