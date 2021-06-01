using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isMoveLeft;
    public GameObject monkey;
    private float range;
    private SpriteRenderer enemyRenderer;
    private Color _colorEnemy;
    
    private void Start()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        _colorEnemy = enemyRenderer.color;
    }

    void Update()
    {
        range = Mathf.Abs(monkey.transform.position.x - transform.position.x);
        if (range < 4)
        {
            enemyRenderer.color = Color.red;
            if (isMoveLeft)
            {
                transform.Translate(new Vector3(-1,0,0)*3*Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(1,0,0)*3*Time.deltaTime);
            }
        }
        else
        {
            enemyRenderer.color = _colorEnemy;
            if (isMoveLeft)
            {
                transform.Translate(new Vector3(-1,0,0)*Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(1,0,0)*Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SquareLeft"))
        {
            isMoveLeft = false;
        }

        if (other.CompareTag("SquareRight"))
        {
            isMoveLeft = true;
        }
    }
}

