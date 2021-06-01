using Unity.Mathematics;
using UnityEngine;

public class TestMonkey : MonoBehaviour
{
    public GameObject T1;
    public GameObject T2;
    

    private void Start()
    {
    }

    void Update()
    {
        transform.SetParent(T2.transform);
        /*Debug.DrawRay(T1.transform.position, T1.transform.forward*6,Color.red);
        Debug.DrawRay(T2.transform.position, T2.transform.forward*6,Color.red);
        Debug.Log(Vector3.Angle(T1.transform.forward,T2.transform.forward));
        Debug.Log(T1.transform.right);
        Debug.Log(T1.transform.forward);*/
        
        Vector3 targetDir = T1.transform.position - T2.transform.position;
        Debug.DrawRay(T1.transform.position, T1.transform.forward*6,Color.red);
        Debug.DrawRay(T2.transform.position, T2.transform.forward*6,Color.red);
        float angle = Vector3.Angle(targetDir, T2.transform.up);
        Debug.Log(angle);
        
    }
}