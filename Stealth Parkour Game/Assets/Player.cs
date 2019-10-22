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
    private int numDirections;


    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity= 1;
        body = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        numDirections = 0;
        vecForceToAdd *= 0;

        if (Input.GetKey("w"))
        {
            numDirections++;
            vecForceToAdd = cam.transform.forward;
              vecForceToAdd.y *=0;
            vecForceToAdd = vecForceToAdd.normalized;
        }
        if (Input.GetKey("d"))
        {
            numDirections++;
            temporaryVec = cam.transform.right;
            temporaryVec.y *= 0;
            temporaryVec = temporaryVec.normalized;
            vecForceToAdd += temporaryVec;
        }
        if (Input.GetKey("s"))
        {
            numDirections++;
            temporaryVec = -cam.transform.forward;
            temporaryVec.y *= 0;
            temporaryVec = temporaryVec.normalized;
            vecForceToAdd += temporaryVec;
        }
        if (Input.GetKey("a"))
        {
            numDirections++;
            temporaryVec = -cam.transform.right;
            temporaryVec.y *= 0;
            temporaryVec = temporaryVec.normalized;
            vecForceToAdd += temporaryVec;
        }
        if (!(Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d")))
        {
            body.velocity.Set(body.velocity.x/10, body.velocity.y, body.velocity.z/10);
        }
        else {
            if (body.velocity.magnitude < 30)
            {
                vecForceToAdd = vecForceToAdd.normalized;
                body.AddForce((vecForceToAdd * 30));
            }
        }

		// CAMERA CONTROLS
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));

		if (cam.transform.eulerAngles.x > 80f && cam.transform.eulerAngles.x < 180f) {
			cam.transform.localEulerAngles = new Vector3 (80f, 0f, 0f);
		}
		if (cam.transform.eulerAngles.x < 280f && cam.transform.eulerAngles.x > 180f) {
			cam.transform.localEulerAngles = new Vector3 (280f, 0f, 0f);
		}

		cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"),0,0));

    }
}
