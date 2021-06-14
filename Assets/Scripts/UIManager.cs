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
    public Animator monkeyAnimatorUI;
    private SpriteRenderer monkeySpriteRendererUI;
    public Sprite monkeySpriteDefault;
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
        monkeySpriteRendererUI = monkey.GetComponent<SpriteRenderer>();
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
        if (loadScene == 0)
        {
            monkey.transform.position = new Vector3(-5.485f,-1.873843f,0);
            jingu.transform.position = new Vector3(-5.3242f,-1.3493f,0);
            monkey.gameObject.SetActive(true);
            jingu.gameObject.SetActive(true);
            Monkey.isGameOver = false;
            monkeySpriteRendererUI.sprite = monkeySpriteDefault;
            buttonScale.SetActive(true);
            UIGameOver.SetActive(false);
            monkeyAnimatorUI.enabled = true;
            Monkey.countCollider = 0;
        }
        else
        {
            SceneManager.LoadScene(loadScene, LoadSceneMode.Single);
            UIGameOver.SetActive(false);
        }
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
