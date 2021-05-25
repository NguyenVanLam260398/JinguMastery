using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms;

public class TestMonkey : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1,0,0)*3*Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1,0,0)*3*Time.deltaTime);
        }
            
    }
}
