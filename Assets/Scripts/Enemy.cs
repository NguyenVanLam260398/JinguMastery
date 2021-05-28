using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isMoveLeft;
    void Update()
    {
        if (isMoveLeft)
        {
            transform.Translate(new Vector3(-1,0,0)*Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(1,0,0)*Time.deltaTime);
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

