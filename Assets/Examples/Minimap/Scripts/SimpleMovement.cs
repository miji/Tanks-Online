using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour 
{
    public float speed = 5;
 
    void Update () 
    {
         float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
         float z = Input.GetAxis("Horizontal") * speed * Time.deltaTime; 
         transform.Translate(0, y, z); 
    }
}
