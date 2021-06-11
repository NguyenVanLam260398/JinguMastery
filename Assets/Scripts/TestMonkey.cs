using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class TestMonkey : MonoBehaviour
{
    public Transform t1;
    public Transform t2;

    private float s = 1;
    // Update is called once per frame
    void Update () 
    {
        if( Input.GetKey(KeyCode.A) )
        {
            transform.DORotate(new Vector3(0,0,90), 10,RotateMode.Fast);
        }
    }
}