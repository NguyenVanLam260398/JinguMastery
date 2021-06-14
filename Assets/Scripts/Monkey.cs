using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    private SpriteRenderer monkeySpriteRenderer;
    public Sprite spriteMonkeyDie;
    
    public static Rigidbody2D monkeyRigrid;
    public static bool isRotationLeft;
    public static bool isMonkeyCollider;
    public static bool isMoveMonkeyLeftRight;
    public static bool isGameOver;
    public static bool isMonkeyCanScaleJingu;
    private bool isMonkeyHaveKey;
    private bool isButtonLeft;
    private bool isButtonRight;
    

    private Jingu _jinGu;
    private Quaternion monkeyRotationDefaut = quaternion.Euler(0,0,0);

    public GameObject UIgameOver;
    public GameObject UINextLeve;
    public GameObject buttonScale;

    private float isLeftRight;
    public static int countCollider = 0; 
    public static int isIntstanceID;

    public Animator monkeyAnimator;
    public Animator doorAnimator;

    public AudioSource isAudioSource;
    public AudioClip isdiMonkey;
    public AudioClip isEatKey;
    public AudioClip isWin;
    public GameObject pivot;
    public GameObject earMonkey;
    void Start()
    {
        monkeySpriteRenderer = GetComponent<SpriteRenderer>();
        monkeyRigrid = GetComponent<Rigidbody2D>();
        _jinGu = FindObjectOfType<Jingu>().GetComponent<Jingu>();
        isMoveMonkeyLeftRight = true;
        isMonkeyCollider = false;
        isButtonLeft = false;
        isButtonRight = false;
        isMonkeyCanScaleJingu = false;
    }
    void Update()
    {
            monkeyRigrid.bodyType = RigidbodyType2D.Kinematic;
            monkeyAnimator.SetBool("isMove",false);
            if (_jinGu.isRotation == false)
            {
                monkeyRigrid.bodyType = RigidbodyType2D.Dynamic;
            }

            if ((isButtonLeft || Input.GetKey(KeyCode.A))&& isMoveMonkeyLeftRight)
            {
                transform.localScale = new Vector3(-1,1,1);
                monkeyAnimator.SetBool("isMove",true);
                transform.Translate(new Vector3(-1,0,0)*2*Time.deltaTime);
                isRotationLeft = true;
                pivot.transform.position = earMonkey.transform.position;
            }

            if ((isButtonRight || Input.GetKey(KeyCode.D)) && isMoveMonkeyLeftRight)
            {
                transform.localScale = new Vector3(1,1,1);
                monkeyAnimator.SetBool("isMove",true);
                transform.Translate(new Vector3(1,0,0)*2*Time.deltaTime);
                isRotationLeft = false;
                pivot.transform.position = earMonkey.transform.position;
            }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        isMonkeyCanScaleJingu = true;
        if (other.collider.CompareTag("Obstacle") || other.collider.CompareTag("EndGame") || other.collider.CompareTag("StartGame"))
        {
            isIntstanceID = other.gameObject.GetInstanceID();
            Debug.Log(isIntstanceID);
            MonkeyCollider();
            
        }
        
        if (other.collider.CompareTag("ObstacleDie"))
        {
            GameOver();
        }

        if (other.collider.CompareTag("Enemy"))
        {
            if ((transform.position.y - 0.4f) > (other.collider.transform.position.y + 0.3f))
            {
                other.gameObject.SetActive(false);
            }
            else
            {
                GameOver();
                other.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
          
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            isMonkeyHaveKey = true;
            isAudioSource.PlayOneShot(isEatKey,0.2f);
        }

        if (other.CompareTag("GameOver"))
        {
            countCollider++;
            _jinGu.gameObject.SetActive(false);
            if (countCollider == 1)
            {
                isAudioSource.PlayOneShot(isdiMonkey,0.2f);
                monkeyRigrid.AddForce(Vector2.up*17,ForceMode2D.Impulse);
                monkeyAnimator.enabled = false;
                monkeySpriteRenderer.sprite = spriteMonkeyDie;
                isMonkeyCollider = true;
            }
            else
            {
                GameOver();
            }
        }
        if (other.CompareTag("Door") && isMonkeyHaveKey)
        {
            MonkeyCollider();
            doorAnimator.SetBool("isDoor",true);
            monkeyAnimator.Play("monkey_Happy");
            StartCoroutine("HidenMonkey");
            isAudioSource.PlayOneShot(isWin,0.4f);
            buttonScale.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isMonkeyCanScaleJingu = false;
    }

    public void MonkeyCollider()
    {
        transform.rotation = monkeyRotationDefaut;
        isMonkeyCollider = true;
    }

    private void GameOver()
    {
        UIgameOver.SetActive(true);
        transform.gameObject.SetActive(false);
        buttonScale.SetActive(false);
        isGameOver = true;
    }

    IEnumerator HidenMonkey()
    {
        yield return new WaitForSeconds(1);
        transform.gameObject.SetActive(false);
        UINextLeve.SetActive(true);
    }

    public void Left()
    {
        isButtonLeft = true;
        isButtonRight = false;
    }

    public void Right()
    {
        isButtonRight = true;
        isButtonLeft = false;
    }

    public void StopMove()
    {
        isButtonLeft = false;
        isButtonRight = false;
        monkeyAnimator.Play("MonkeyIdle");
    }
}
