using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isMoveLeft;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    private void OnCollisionEnter2D(Collision2D other)
    {
       
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

