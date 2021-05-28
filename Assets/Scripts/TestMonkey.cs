using Unity.Mathematics;
using UnityEngine;

public class TestMonkey : MonoBehaviour
{
    private Quaternion target_90 = quaternion.Euler(0,0,90);
    private Quaternion target_am90 = quaternion.Euler(0,0,-90);
    public GameObject monkey;
    public GameObject pivot;
    public GameObject endPivot;
    void Update()
    {
        monkey.transform.position = endPivot.transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.rotation = Quaternion.Lerp(transform.rotation, target_90, 10*Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target_am90, 10*Time.deltaTime);
        }
    }
}
