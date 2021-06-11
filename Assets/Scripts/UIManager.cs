using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static bool isPlay;
    public GameObject UIStart;
    public GameObject UIGameOver;
    public GameObject UINextLeve;
    private int loadScene;
    public GameObject monkey;
    public GameObject jingu;
    public GameObject buttonScale;
    private void Awake()
    {
        loadScene = SceneManager.GetActiveScene().buildIndex;
        if (loadScene >= 1)
        {
            ButtonGameStart();
        }
    }

    private void Start()
    {
        isPlay = false;
    }

    public void ButtonGameStart()
    {
        isPlay = true;
        UIStart.SetActive(false);
        monkey.SetActive(true);
        jingu.SetActive(true);  
        buttonScale.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(loadScene, LoadSceneMode.Single);
        UIGameOver.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(loadScene+1, LoadSceneMode.Single);
        loadScene = SceneManager.GetActiveScene().buildIndex;
        UINextLeve.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            this.NextLevel();  
        }
    }
}
