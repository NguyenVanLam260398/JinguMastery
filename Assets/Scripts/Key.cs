using UnityEngine;

public class Key : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0,100*Time.deltaTime,0));
    }
}
