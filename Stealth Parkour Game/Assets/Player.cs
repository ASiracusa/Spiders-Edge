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
    private int ArmsBackForth;
    private Vector3 grappleReturnTo;


    [SerializeField]
    private GameObject rightLeg;
    [SerializeField]
    private GameObject leftLeg;
    [SerializeField]
    private GameObject rightArm;
    [SerializeField]
    private GameObject leftArm;
    [SerializeField]
    private GameObject grappler;




    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity= 1;
        body = GetComponent<Rigidbody>();
        ArmsBackForth = -1;


    }

    // Update is called once per frame
    void Update()
    {
        //looking around
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
        cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"),0,0));


        //WASD and jumping movement
        vecForceToAdd *= 0;

        if (jumpCounter < 30)
            jumpCounter++;

        if (isGrounded)
        {

            if (Input.GetKey("space") && jumpCounter >= 30)
            {
                body.AddForce(new Vector3(0, 500, 0));
                jumpCounter = 0;
            }

            if (Input.GetKey("w"))
            {
                vecForceToAdd = cam.transform.forward;
                vecForceToAdd.y *= 0;
                vecForceToAdd = vecForceToAdd.normalized;

                if (Mathf.Sqrt(rightLeg.transform.rotation.x * rightLeg.transform.rotation.x + rightLeg.transform.rotation.z * rightLeg.transform.rotation.z) > .3)
                    ArmsBackForth *= -1;

                rightLeg.transform.Rotate(new Vector3(3 * ArmsBackForth,0,0));
                leftLeg.transform.Rotate(new Vector3(-3 * ArmsBackForth, 0, 0));
                rightArm.transform.Rotate(new Vector3(-3 * ArmsBackForth, 0, 0));
                leftArm.transform.Rotate(new Vector3(3 * ArmsBackForth, 0, 0));
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
                body.velocity.Set(body.velocity.x / 200, body.velocity.y, body.velocity.z / 200);
            }
            else
            {
                if (body.velocity.magnitude < 30)
                {
                    vecForceToAdd = vecForceToAdd.normalized;
                    body.AddForce((vecForceToAdd * 30));
                }
            }

            if (Input.GetKey("e"))
            {


                grappler.transform.rotation = cam.transform.rotation;
                RaycastHit hit;
                if (Physics.Raycast(grappler.transform.position, grappler.transform.forward, out hit, 100))
                {
                    grappler.transform.position = hit.transform.position;

                }

            }
            if (!Input.GetKey("e"))
            {

                grappler.transform.position = body.transform.position;
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
