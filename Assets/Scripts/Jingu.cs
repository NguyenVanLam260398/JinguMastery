using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Jingu : MonoBehaviour
{
    private Rigidbody2D jinguRB;
    public Button skda;
    
    private Quaternion target_right = Quaternion.Euler(0,0,-89);
    private Quaternion target_Left = Quaternion.Euler(0,0,89);
    private Quaternion target_0 = quaternion.Euler(0,0,0);
    private Quaternion target_am90 = quaternion.Euler(0,0,-90);
    
    public static bool _isFinishScale;
    public  bool isRotation;
    private bool isReplayQuaternionJingu = false;
    private bool isScaleJingu;
    private bool isPlayAudio;
    private bool isReloadAudio;
    private bool isReplayQuaternionDone;
    private bool scaleJingu;
    private bool doneScaleJingu;
    
    private Vector3 monkeyLocalPosi = new Vector3(0,0.5f,0);
    private Vector3 monkeyLocalPosiDefaut = new Vector3(0,0,0);
    private Vector3 localScalePivotDefaut = new Vector3(1,1,1);
    
    public GameObject startPivot;
    public GameObject endPivot;
    public GameObject pivot;
    public GameObject monkey;
    public GameObject handMonkey;
    
    private int instanID;
    public  int curentObstacleInstanceID;

    public AudioSource isAudioSource;
    public AudioClip isAudioScaleJingu;
    
    void Start()
    {
        jinguRB = GetComponent<Rigidbody2D>();
        transform.SetParent(handMonkey.transform);
        transform.localPosition = new Vector3(0,0,0);
        isScaleJingu = true;
        isPlayAudio = false;
        isReloadAudio = true;
    }
    void Update()
    {
        //if (UIManager.isPlay)
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
                   if (pivot.transform.localScale.y <20)
                   {
                       pivot.transform.localScale = pivot.transform.localScale + new Vector3(0, 10*Time.deltaTime, 0);
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
                monkey.transform.SetParent(endPivot.transform);
                monkey.transform.localPosition = monkeyLocalPosiDefaut;
                monkey.transform.localPosition = monkeyLocalPosi;
                isRotation = true;
                InvokeRepeating("QuaternionJingu",1,0.0001f);        
            }
            if (isReplayQuaternionJingu && pivot.transform.localScale.y > 0.4f && Monkey.isMonkeyCollider)    
            {
                ReplayQuaternionJingu();
            }
            if (pivot.transform.localScale.y <= 0.4f && Monkey.isMonkeyCollider)
            {
                transform.SetParent(null);
                _isFinishScale = true;
                transform.rotation = target_0;
                isReplayQuaternionJingu = false;    
                pivot.transform.rotation = target_0;
                transform.SetParent(handMonkey.transform);
                isScaleJingu = true;
                transform.localPosition = new Vector3(0,0,0);
                scaleJingu = false;
                doneScaleJingu = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("ObstacleDie") || other.CompareTag("StartGame") || other.CompareTag("EndGame"))
        {
            Monkey.isMoveMonkeyLeftRight = true;
            instanID = other.gameObject.GetInstanceID();
            if (instanID != curentObstacleInstanceID)
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
            if (pivot.transform.rotation == target_Left)
            {
                Monkey.isMoveMonkeyLeftRight = true;
                DetachMonkey();
                ReplayQuaternionJingu();
            }
            monkey.transform.rotation = Quaternion.Lerp(monkey.transform.rotation,target_am90,20*Time.deltaTime);
        }

        if (isRotation && Monkey.isRotationLeft == false)
        {
            pivot.transform.rotation = Quaternion.Lerp(transform.rotation, target_right,Time.deltaTime);
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
        curentObstacleInstanceID = instanID;
    }
    public void DetachMonkey()
    {
        monkey.transform.SetParent(null);
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
