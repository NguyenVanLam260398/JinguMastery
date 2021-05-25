using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject monkeyPos;

    private void Update()
    {
        //if (UIManager.isPlay)
        {
            if (monkeyPos.transform.position.x < (transform.position.x -4) || monkeyPos.transform.position.x > (transform.position.x + 4))
            {
                SetCamera();
            }
            if (monkeyPos.transform.position.y < (transform.position.y -2) || monkeyPos.transform.position.y > (transform.position.y + 2))
            {
                SetCamera();
            }
        }
    }

    private void SetCamera()
    {
        Vector3 position = this.transform.position;
        if (Monkey.isRotationLeft)
        {
            position.x = Mathf.Lerp(transform.position.x, (monkeyPos.transform.position.x +2), Time.deltaTime);
        }
        else
        {
            position.x = Mathf.Lerp(transform.position.x, monkeyPos.transform.position.x - 2, Time.deltaTime);
        }
        position.y = Mathf.Lerp(transform.position.y, monkeyPos.transform.position.y, Time.deltaTime);
        this.transform.position = position;
    }
}
