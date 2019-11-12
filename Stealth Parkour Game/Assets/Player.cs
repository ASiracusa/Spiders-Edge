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
    private float currentVelocity;
    private RaycastHit hit;
    private Vector3 currentDirection;


    //variables for swinging
    private float height;
    private float distanceFromRotationPoint;
    private float amountToRotate;

    private int grappleLayer;

    [SerializeField]
    private GameObject rightLeg;
    [SerializeField]
    private GameObject leftLeg;
    [SerializeField]
    private GameObject rightArm;
    [SerializeField]
    private GameObject leftArm;
    [SerializeField]
    private bool hasUnGrappled;   
    [SerializeField]
    Canvas grappledText;
   



    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity= 1;
        body = GetComponent<Rigidbody>();
        ArmsBackForth = -1;
        hasUnGrappled = true;
        grappledText.enabled = false;
        grappleLayer = 9;
    }




    // Update is called once per frame
    void Update()
    {


       

    
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

                rightLeg.transform.Rotate(new Vector3(3 * ArmsBackForth, 0, 0));
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
                    body.AddForce((vecForceToAdd * 20));
                }
            }            
        }





        //this initiates a grapple
            if (Input.GetMouseButtonDown(4))
            {                               
                if (Physics.Raycast(body.transform.position, cam.transform.forward, out hit, 100) && hasUnGrappled && hit.transform.gameObject.layer == grappleLayer)
                {
                    
                    currentVelocity = body.velocity.magnitude;
                    currentDirection = body.velocity;
                    distanceFromRotationPoint = (hit.point - body.transform.position).magnitude;
                Debug.Log(distanceFromRotationPoint);
                                 
                    hasUnGrappled = false;                                       
                    grappledText.enabled = true;                    
                }
            }

        
        

            //this is what happens while we're grappled
            if (!hasUnGrappled)
            {
     
                if((hit.point - body.transform.position).magnitude > distanceFromRotationPoint)
                {
                    body.AddForce(body.velocity.magnitude  * (hit.point - body.position).normalized);
                }

         
            }


            //ends the grapple
            if (Input.GetMouseButtonUp(4))
            {

               hasUnGrappled = true;
                body.isKinematic = false;
                body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                grappledText.enabled = false;

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
