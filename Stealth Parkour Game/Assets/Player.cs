using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float mouseSensitivity;
    private Rigidbody body;
    public GameObject cam;
   

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity= 1;
        body = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
        cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"),0,0));
        if (Input.GetKey("w"))
        {
            body.AddForce((Vector3.forward)*3);
        }



    }
}
