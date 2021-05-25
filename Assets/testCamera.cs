using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCamera : MonoBehaviour
{
    public GameObject isMonkey;

    [Range(1,10)]
    public float smoothFactor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        flowGameObject();
    }

    private void flowGameObject()
    {
        Vector3 targetPosition = isMonkey.transform.position + new Vector3(0, 0, 10);
        Vector3 smoothCamera = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothCamera;
    }
}
