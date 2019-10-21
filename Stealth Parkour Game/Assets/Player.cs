using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float mouseSensitivity;
    private Rigidbody body;
    public GameObject cam;
    private Vector3 vecForceToAdd;
    private Vector3 temporaryVec;
    private bool isGrounded;
    private float heightOverGround;
    private int jumpCounter;




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


        vecForceToAdd *= 0;

        if (isGrounded)
        {

            if (jumpCounter <30)
            jumpCounter++;

            if (Input.GetKey("space") && jumpCounter >= 30)
            {
                body.AddForce(new Vector3(0, 1000, 0));
                jumpCounter = 0;
            }
            if (Input.GetKey("w"))
            {

                vecForceToAdd = cam.transform.forward;
                vecForceToAdd.y *= 0;
                vecForceToAdd = vecForceToAdd.normalized;
            }
            if (Input.GetKey("d"))
            {

                temporaryVec = cam.transform.right;
                temporaryVec.y *= 0;
                temporaryVec = temporaryVec.normalized;
                vecForceToAdd += temporaryVec;
            }
            if (Input.GetKey("s"))
            {

                temporaryVec = -cam.transform.forward;
                temporaryVec.y *= 0;
                temporaryVec = temporaryVec.normalized;
                vecForceToAdd += temporaryVec;
            }
            if (Input.GetKey("a"))
            {

                temporaryVec = -cam.transform.right;
                temporaryVec.y *= 0;
                temporaryVec = temporaryVec.normalized;
                vecForceToAdd += temporaryVec;
            }
            if (!(Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d")))
            {
                body.velocity.Set(body.velocity.x / 10, body.velocity.y, body.velocity.z / 10);
            }
            else
            {
                if (body.velocity.magnitude < 30)
                {
                    vecForceToAdd = vecForceToAdd.normalized;
                    body.AddForce((vecForceToAdd * 30));
                }
            }
        }



    }


    // tests for grounding adapted from https://stackoverflow.com/questions/44539237/unity-checking-if-the-player-is-grounded-not-working
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) {
            isGrounded = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }

}
