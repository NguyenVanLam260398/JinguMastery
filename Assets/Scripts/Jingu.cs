using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Jingu : MonoBehaviour
{
    private Rigidbody2D jinguRB;
    private SpriteRenderer monkeySpriteRenderer;
    public Sprite spriteOne;
    public Sprite spriteTwo;
    private Sprite spriteDefautl;
    
    private Quaternion target_right = Quaternion.Euler(0,0,-89);
    private Quaternion target_Left = Quaternion.Euler(0,0,89);
    private Quaternion target_0 = quaternion.Euler(0,0,0);
    
    public static bool _isFinishScale;
    public  bool isRotation;
    private bool isReplayQuaternionJingu = false;
    private bool isScaleJingu;
    private bool isPlayAudio;
    private bool isReloadAudio;
    private bool isReplayQuaternionDone;
    private bool scaleJingu;
    private bool doneScaleJingu;
    private bool isAnimation;
    
    private Vector3 localScalePivotDefaut = new Vector3(1,1,1);
    
    public GameObject startPivot;
    public GameObject endPivot;
    public GameObject pivot;
    public GameObject monkey;
    public GameObject handMonkey;
    public GameObject EarMonkey;
    
    private int instanID;
    public  int curentObstacleInstanceID;

    public AudioSource isAudioSource;
    public AudioClip isAudioScaleJingu;
    
    public Animator monkeyJumb;
    
    void Start()
    {
        monkeySpriteRenderer = monkey.GetComponent<SpriteRenderer>();
        spriteDefautl = monkeySpriteRenderer.sprite;
        jinguRB = GetComponent<Rigidbody2D>();
        transform.SetParent(monkey.transform);
        transform.localPosition = new Vector3(0.1f,-0.25f,0);
        isScaleJingu = true;
        isPlayAudio = false;
        isReloadAudio = true;
    }
    void Update()
    {
        
        if (UIManager.isPlay)
        {
            if(scaleJingu && isScaleJingu)
            {
                   Monkey.isMonkeyCollider = false;
                   Monkey.isMoveMonkeyLeftRight = false;
                   transform.SetParent(null);
                   pivot.transform.position = startPivot.transform.position;
                   transform.SetParent(pivot.transform);
                   isPlayAudio = true;
                   PlayAudio();
                   isAnimation = true;
                   if (pivot.transform.localScale.y <20)
                   {
                       pivot.transform.localScale = pivot.transform.localScale + new Vector3(0, 4*Time.deltaTime, 0);
                   }

                   if (pivot.transform.localScale.y >= 20)
                   {
                      pivot.transform.localScale = localScalePivotDefaut;
                   }
            }
            if (doneScaleJingu && isScaleJingu)
            {
                
                isReloadAudio = true;
                isScaleJingu = false;
                isRotation = true;
                InvokeRepeating("QuaternionJingu",0.2f,0.0001f); 
            }
            if (isReplayQuaternionJingu && pivot.transform.localScale.y > 0.1f && Monkey.isMonkeyCollider)    
            {
                ReplayQuaternionJingu();
            }
            if (pivot.transform.localScale.y <= 0.1f && Monkey.isMonkeyCollider)
            {
                transform.SetParent(null);
                _isFinishScale = true;
                transform.rotation = target_0;
                isReplayQuaternionJingu = false;    
                pivot.transform.rotation = target_0;
                transform.SetParent(EarMonkey.transform);
                isScaleJingu = true;
                transform.localPosition = new Vector3(0,0,0);
                scaleJingu = false;
                doneScaleJingu = false;
                if (isAnimation)
                {
                    monkeyJumb.Play("MonkeyScaleJungu");
                    isAnimation = false;
                    Debug.Log("Zo function roi");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("ObstacleDie") || other.CompareTag("StartGame") || other.CompareTag("EndGame"))
        {
            Monkey.isMoveMonkeyLeftRight = true;
            CancelInvoke("QuaternionJingu");
            instanID = other.gameObject.GetInstanceID();
            if (instanID != Monkey.isIntstanceID)
            {
                jinguColliderObstacle();
            }
        }
    }

    private void QuaternionJingu()
    {
        if (isRotation && Monkey.isRotationLeft)
        {
            pivot.transform.rotation = Quaternion.Lerp(transform.rotation, target_Left,Time.deltaTime);
            monkey.transform.position = endPivot.transform.position;
            if (pivot.transform.rotation.z > 0 && pivot.transform.rotation.z < 0.3f)
            {
                monkeySpriteRenderer.sprite = spriteOne;
            }else if (pivot.transform.rotation.z >= 0.3f && pivot.transform.rotation.z < 0.5f)
            {
                monkeySpriteRenderer.sprite = spriteTwo;
            }else
            {
                monkeySpriteRenderer.sprite = spriteDefautl;
            }
            if (pivot.transform.rotation == target_Left)
            {
                Monkey.isMoveMonkeyLeftRight = true;
                DetachMonkey();
                ReplayQuaternionJingu();
            }
        }

        if (isRotation && Monkey.isRotationLeft == false)
        {
            pivot.transform.rotation = Quaternion.Lerp(transform.rotation, target_right,Time.deltaTime);
            monkey.transform.position = endPivot.transform.position;
            if (pivot.transform.rotation.z < 0 && pivot.transform.rotation.z > -0.3f)
            {
                monkeySpriteRenderer.sprite = spriteOne;
            }else if (pivot.transform.rotation.z <= -0.3f && pivot.transform.rotation.z > -0.5f)
            {
                monkeySpriteRenderer.sprite = spriteTwo;
            }else
            {
                monkeySpriteRenderer.sprite = spriteDefautl;
            }
            if (pivot.transform.rotation == target_right)
            {
                Monkey.isMoveMonkeyLeftRight = true;
                DetachMonkey();
                ReplayQuaternionJingu();
            }
        }
    }
    private void ReplayQuaternionJingu()
    {
        transform.SetParent(pivot.transform);
        pivot.transform.localScale = pivot.transform.localScale - new Vector3(0, 10*Time.deltaTime, 0);
        if (Monkey.isRotationLeft)
        {
            pivot.transform.rotation = Quaternion.Lerp(transform.rotation,target_Left,10*Time.deltaTime);
        }
        else
        {
            pivot.transform.rotation = Quaternion.Lerp(transform.rotation,target_right,10*Time.deltaTime);
        }
    }

    public void jinguColliderObstacle()
    {
        DetachMonkey();
    }
    public void DetachMonkey()
    {
        monkey.transform.SetParent(null);
        monkey.transform.rotation = Quaternion.Lerp(monkey.transform.rotation,target_0,1);
        isRotation = false;
        isReplayQuaternionJingu = true;
        transform.SetParent(null);
        pivot.transform.position = endPivot.transform.position;
    }

    private void PlayAudio()
    {
        if (isPlayAudio && isReloadAudio)
        {
            isAudioSource.PlayOneShot(isAudioScaleJingu,0.2f);
            isReloadAudio = false;
        }
    }

    public void ScaleJingu()
    {
        scaleJingu = true;
        doneScaleJingu = false;
    }

    public void DoneScaleJingu()
    {
        scaleJingu = false;
        doneScaleJingu = true;
    }
}
