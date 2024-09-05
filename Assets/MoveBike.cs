using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBike : MonoBehaviour
{

    public Vector3 point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float speed = 2;
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0, z);
        transform.Translate(movement * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.M))
        {
            transform.RotateAround(point, Vector3.up, 5 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.N))
            transform.RotateAround(point, -Vector3.up, 5 * Time.deltaTime);

    }
}

