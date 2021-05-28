using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public static Rigidbody2D monkeyRigrid;
    public static bool isRotationLeft;
    public static bool isMonkeyCollider;
    public static bool isMoveMonkeyLeftRight;
    private bool isMonkeyHaveKey;
    private bool isButtonLeft;
    private bool isButtonRight;

    private Jingu _jinGu;
    private Quaternion monkeyRotationDefaut = quaternion.Euler(0,0,0);

    public GameObject UIgameOver;
    public GameObject UINextLeve;

    private float isLeftRight;
    public static int isIntstanceID;

    public Animator monkeyAnimator;
    public Animator doorAnimator;

    public AudioSource isAudioSource;
    public AudioClip isdiMonkey;
    public AudioClip isEatKey;
    public AudioClip isWin;
    void Start()
    {
        monkeyRigrid = GetComponent<Rigidbody2D>();
        _jinGu = FindObjectOfType<Jingu>().GetComponent<Jingu>();
        isMoveMonkeyLeftRight = true;
        isMonkeyCollider = false;
        isButtonLeft = false;
        isButtonRight = false;
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
                
            }

            if ((isButtonRight || Input.GetKey(KeyCode.D)) && isMoveMonkeyLeftRight)
            {
                transform.localScale = new Vector3(1,1,1);
                monkeyAnimator.SetBool("isMove",true);
                transform.Translate(new Vector3(1,0,0)*2*Time.deltaTime);
                isRotationLeft = false;
            }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.collider.CompareTag("Obstacle") || other.collider.CompareTag("EndGame") || other.collider.CompareTag("StartGame"))
        {
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
            GameOver();
        }
        if (other.CompareTag("Door") && isMonkeyHaveKey)
        {
            isAudioSource.PlayOneShot(isWin,0.5f);
            MonkeyCollider();
            doorAnimator.SetBool("isDoor",true);
            StartCoroutine("HidenMonkey");
        }
    }

    public void MonkeyCollider()
    {
        transform.rotation = monkeyRotationDefaut;
        isMonkeyCollider = true;
        
    }

    private void GameOver()
    {
        isAudioSource.PlayOneShot(isdiMonkey,0.2f);
        UIgameOver.SetActive(true);
        transform.gameObject.SetActive(false);
        Debug.Log("Game Over");
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
    }
}
