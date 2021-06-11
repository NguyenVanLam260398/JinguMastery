using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Jingu : MonoBehaviour
{
    private Rigidbody2D jinguRB;
    private SpriteRenderer monkeySpriteRenderer;
    public  Sprite spriteOne;
    public  Sprite spriteTwo;
    private Sprite spriteDefautl;
    
    private Quaternion target_right = Quaternion.Euler(0,0,-90);
    private Quaternion target_Left = Quaternion.Euler(0,0,90);
    private Quaternion target_0 = quaternion.Euler(0,0,0);
    
    private Vector3 localScalePivotDefaut = new Vector3(1,1,1);
    
    public  bool _isFinishScale;
    public  bool isRotation;
    private bool isReplayQuaternionJingu = false;
    private bool isScaleJingu;
    private bool isPlayAudio;
    private bool isReloadAudio;
    private bool isReplayQuaternionDone;
    private bool scaleJingu;
    private bool doneScaleJingu;
    private bool isAnimation;
    private bool isJinguCollider;
    
    public GameObject startPivot;
    public GameObject endPivot;
    public GameObject pivot;
    public GameObject monkey;
    public GameObject handMonkey;
    public GameObject EarMonkey;
    public Transform t1;
    public Transform t2;
    
    
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
        transform.localPosition = new Vector3(0.2f,0.32f,0);
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
                   if (pivot.transform.localScale.y < 20)
                   {
                       //transform.DetachChildren();                 
                       t1.SetParent(null);
                       t2.SetParent(null);
                       pivot.transform.localScale = pivot.transform.localScale + new Vector3(0,10*Time.deltaTime, 0); 
                       t1.parent = endPivot.transform;
                       t1.localPosition = new Vector3(0,0,0);
                       t2.parent = startPivot.transform;
                       t2.localPosition = new Vector3(0,0,0);
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
                QuaternionjinguDotween(isJinguCollider);
            }
            if (isReplayQuaternionJingu && pivot.transform.localScale.y > 0.8f && Monkey.isMonkeyCollider)    
            {
                ReplayQuaternionJingu();
            }
            if (pivot.transform.localScale.y <= 0.8f && Monkey.isMonkeyCollider)
            {
                transform.SetParent(null);
                _isFinishScale = true;
                transform.rotation = target_0;
                isReplayQuaternionJingu = false;    
                pivot.transform.rotation = target_0;
                transform.SetParent(handMonkey.transform);
                isScaleJingu = true;
                transform.localPosition = new Vector3(0.2f,0.852f,0);
                scaleJingu = false;
                doneScaleJingu = false;
                isJinguCollider = true;
                if (isAnimation)
                {
                    monkeyJumb.Play("MonkeyScaleJungu");
                    isAnimation = false;
                }
            }

            if (Monkey.isMonkeyCollider)
            {
                DOTween.PauseAll();
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
            monkey.transform.position = endPivot.transform.position;
            if (pivot.transform.localScale.y >= 2)
            {
                if (pivot.transform.rotation.z > 0 && pivot.transform.rotation.z < 0.5f)
                {
                    monkeySpriteRenderer.sprite = spriteOne;
                }else if (pivot.transform.rotation.z >= 0.5f && pivot.transform.rotation.z < 0.7f)
                {
                    monkeySpriteRenderer.sprite = spriteTwo;
                }else
                {
                    monkeySpriteRenderer.sprite = spriteDefautl;
                }
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
            monkey.transform.position = endPivot.transform.position;
            if(pivot.transform.localScale.y >= 2)
            {
                if (pivot.transform.rotation.z < 0 && pivot.transform.rotation.z > -0.5f)
                {
                    monkeySpriteRenderer.sprite = spriteOne;
                }else if (pivot.transform.rotation.z <= -0.5f && pivot.transform.rotation.z > -0.7f)
                {
                    monkeySpriteRenderer.sprite = spriteTwo;
                }else
                {
                    monkeySpriteRenderer.sprite = spriteDefautl;
                }
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
        t1.SetParent(null);
        t2.SetParent(null);
        pivot.transform.localScale = pivot.transform.localScale - new Vector3(0,7*Time.deltaTime, 0); 
        t1.parent = endPivot.transform;
        t1.localPosition = new Vector3(0,0,0);
        t2.parent = startPivot.transform;
        t2.localPosition = new Vector3(0,0,0);
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

    private void QuaternionjinguDotween(bool colliderr)
    {
        colliderr = false;
        if (isRotation && Monkey.isRotationLeft)
        {
            pivot.transform.DORotate(new Vector3(0, 0, 90), 2f, RotateMode.Fast);
        }

        if (isRotation && Monkey.isRotationLeft == false)
        {
            pivot.transform.DORotate(new Vector3(0, 0, -90), 2f, RotateMode.Fast);
        }
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
