using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Jingu : MonoBehaviour
{
    public enum JinguState
    {
        Init,
        ReadyToScale,
        ScalingOut,
        Rotating,
        StopRotating,
        ScalingIn
    }

    private JinguState mJinguState = JinguState.Init;

    public JinguState jinguState
    {
        get { return mJinguState; }
        set
        {
            Debug.Log("State from: " + mJinguState + " => " + value);
            mJinguState = value;
        }
    }
    private SpriteRenderer monkeySpriteRenderer;
    public  Sprite spriteOne;
    public  Sprite spriteTwo;
    private Sprite spriteDefault;
    
    private Quaternion rightAngle = Quaternion.Euler(0,0,-90);
    private Quaternion leftAngle = Quaternion.Euler(0,0,90);
    private Quaternion middleAngle = quaternion.Euler(0,0,0);
    
    private Vector3 defaultPivotScale = new Vector3(1,1,1);
    public bool  isRotating { get; private set; }
    private bool isJinguScalingIn = false;
    private bool isJinguScalingOut;
    private bool isScalingOutAudioPlaying;
    private bool isScaleButtonDown;
    private bool isScaleButtonUp;
    private bool mIsMonkeyCollideWithObstacle;

    private bool isMonkeyCollideWithObstacle
    {
        get
        {
            return mIsMonkeyCollideWithObstacle;
        }
        set
        {
            Debug.Log("Is Monkey Collide: " + mIsMonkeyCollideWithObstacle + "=>" + value);
            mIsMonkeyCollideWithObstacle = value;
        }
    }

    public GameObject startPivot;
    public GameObject endPivot;
    public GameObject pivot;
    public GameObject monkey;
    public GameObject handMonkey;
    public Transform t1;
    public Transform t2;
    
    
    private int instanID;

    public AudioSource isAudioSource;
    public AudioClip isAudioScaleJingu;
    
    public Animator monkeyJumb;
    private Sequence rotateSequence = null;
    private bool isFirstTimeCollide = true;
    
    void Start()
    {
        monkeySpriteRenderer = monkey.GetComponent<SpriteRenderer>();
        spriteDefault = monkeySpriteRenderer.sprite;
        transform.SetParent(monkey.transform);
        transform.localPosition = new Vector3(0.2f,0.32f,0);
        isJinguScalingOut = true;
        isScalingOutAudioPlaying = false;
        Monkey.onMonkeyCollide += OnMonkeyCollideWithObstacle;
    }

    private void OnDestroy()
    {
        Monkey.onMonkeyCollide -= OnMonkeyCollideWithObstacle;
    }

    void Update()
    {
        if (UIManager.isPlay)
        {
            switch (jinguState)
            {
                case JinguState.Init:
                    Debug.Log("Init Jingu");
                    // Khoi tao gay, monkey
                    transform.SetParent(null);
                    transform.rotation = middleAngle;
                    pivot.transform.rotation = middleAngle;
                    transform.SetParent(handMonkey.transform);
                    transform.localPosition = new Vector3(0.35f,0.52f,0);
                    jinguState = JinguState.ReadyToScale;
                    rotateSequence = null;
                    isMonkeyCollideWithObstacle = false;
                    break;
                case JinguState.ScalingOut:
                    Debug.Log("Scaling out...");
                    // Logic gian giay ra
                    transform.SetParent(null);
                    pivot.transform.position = startPivot.transform.position;
                    transform.SetParent(pivot.transform);
                    PlayAudio();
                    if (pivot.transform.localScale.y < 20)
                    {
                        t1.SetParent(null);
                        t2.SetParent(null);
                        pivot.transform.localScale = pivot.transform.localScale + new Vector3(0,6*Time.deltaTime, 0); 
                        t1.parent = endPivot.transform;
                        t1.localPosition = new Vector3(0,0,0);
                        t2.parent = startPivot.transform;
                        t2.localPosition = new Vector3(0,0,0);
                    }

                    if (pivot.transform.localScale.y >= 20)
                    {
                        pivot.transform.localScale = defaultPivotScale;
                    }
                    break;
                case JinguState.Rotating:
                    // Logic xu ly khi dung scaling
                    Debug.Log("Rotating...");
                    isScalingOutAudioPlaying = false;
                    RotateJingu();
                    UpdateMonkeyAnimationOnRotating();
                    break;
                case JinguState.ScalingIn:
                    Debug.Log("Scaling in...");
                    // Login thu gay ve
                    if (pivot.transform.localScale.y > 1f)    
                    {
                        ReplayQuaternionJingu();
                    }
                    else
                    {
                        Debug.Log("Back to init state");
                        jinguState = JinguState.Init;
                    }
                    break;
            }
        }
    }

    private void OnMonkeyCollideWithObstacle()
    {
        Debug.Log("On Monkey Collide With Obstacle");
        if (isFirstTimeCollide)
        {
            isFirstTimeCollide = false;
            return;
        }
        
        jinguState = JinguState.ScalingIn;
        isMonkeyCollideWithObstacle = true;
        Monkey.isMoveMonkeyLeftRight = true;
        DetachMonkey();
        ReplayQuaternionJingu();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("ObstacleDie") || other.CompareTag("StartGame") || other.CompareTag("EndGame"))
        {
            Debug.Log("Jingu Collide: " + other.gameObject.GetInstanceID() + ", " + Monkey.isIntstanceID);
            if (other.gameObject.GetInstanceID() != Monkey.isIntstanceID)
            {
                jinguColliderObstacle();
            }
            
            // Stop rotating
            if (rotateSequence != null)
            {
                rotateSequence.Kill();
                rotateSequence = null;
            }
        }
    }
    private void UpdateMonkeyAnimationOnRotating()
    {
        if (rotateSequence == null)
        {
            return;
        }

        if (Monkey.isRotationLeft)
        {
            if (pivot.transform.localScale.y >= 2)
            {
                if (pivot.transform.rotation.z > 0 && pivot.transform.rotation.z < 0.4f)
                {
                    monkeySpriteRenderer.sprite = spriteOne;
                }else if (pivot.transform.rotation.z >= 0.4f && pivot.transform.rotation.z < 0.6f)
                {
                    monkeySpriteRenderer.sprite = spriteTwo;
                }else
                {
                    monkeySpriteRenderer.sprite = spriteDefault;
                }
            }
            if (pivot.transform.rotation == leftAngle)
            {
                Monkey.isMoveMonkeyLeftRight = true;
                DetachMonkey();
                ReplayQuaternionJingu();
            }
        }
        if (Monkey.isRotationLeft == false)
        {
            if(pivot.transform.localScale.y >= 2)
            {
                if (pivot.transform.rotation.z < 0 && pivot.transform.rotation.z > -0.4f)
                {
                    monkeySpriteRenderer.sprite = spriteOne;
                }else if (pivot.transform.rotation.z <= -0.4f && pivot.transform.rotation.z > -0.6f)
                {
                    monkeySpriteRenderer.sprite = spriteTwo;
                }else
                {
                    monkeySpriteRenderer.sprite = spriteDefault;
                }
            }
            if (pivot.transform.rotation == rightAngle)
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
        pivot.transform.localScale = pivot.transform.localScale - new Vector3(0,6*Time.deltaTime, 0); 
        t1.parent = endPivot.transform;
        t1.localPosition = new Vector3(0,0,0);
        t2.parent = startPivot.transform;
        t2.localPosition = new Vector3(0,0,0);
    }
    public void jinguColliderObstacle()
    {
        DetachMonkey();
    }
    public void DetachMonkey()
    {
        monkey.transform.SetParent(null);
        monkey.transform.rotation = Quaternion.Lerp(monkey.transform.rotation,middleAngle,1);
        isRotating = false;
        isJinguScalingIn = true;
        transform.SetParent(null);
        pivot.transform.position = endPivot.transform.position;
    }

    private void RotateJingu()
    {
        if (rotateSequence == null)
        {
            rotateSequence = DOTween.Sequence();
            Tweener rotateTweener = null;
            if (Monkey.isRotationLeft)
            {
                rotateTweener = pivot.transform.DORotate(new Vector3(0, 0, 90), 1);
            }

            if (Monkey.isRotationLeft == false)
            {
                rotateTweener = pivot.transform.DORotate(new Vector3(0, 0, -90), 1);
            }

            if (rotateTweener != null)
            {
                rotateSequence.Append(rotateTweener);
                rotateTweener.onUpdate = delegate
                {
                    if (rotateSequence == null)
                    {
                        return;
                    }

                    monkey.transform.position = endPivot.transform.position;
                };
                rotateTweener.onComplete = OnRotatingComplete;
            }
        }
    }

    private void OnRotatingComplete()
    {
        Debug.Log("On Rotating Complete " + (rotateSequence == null));
        if (rotateSequence != null)
        {
            jinguState = JinguState.StopRotating;
        }
    }

    private void PlayAudio()
    {
        if (!isScalingOutAudioPlaying)
        {
            isAudioSource.PlayOneShot(isAudioScaleJingu,0.2f);
            isScalingOutAudioPlaying = true;
        }
    }
    public void ScaleJingu()
    {
        jinguState = JinguState.ScalingOut;
    }
    
    public void DoneScaleJingu()
    {
        jinguState = JinguState.Rotating;
    }
}
