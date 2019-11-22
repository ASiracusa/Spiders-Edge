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
    private float deltaT;
    private float oldTime;
    private float walkingSpeed;
    private int currentLevelNumber;
    private GameObject currentLevel;
    private float timeSinceLastWin;
    
  
	

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
    [SerializeField]
    GameObject[] levels;
    [SerializeField]
    Vector3[] startingPositions;
   



    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        StartCoroutine(KeepUsMoving());
        currentLevelNumber = 0;
        currentLevel = Instantiate(levels[currentLevelNumber]);
        transform.position = startingPositions[currentLevelNumber];
        mouseSensitivity= 1;
        ArmsBackForth = -1;
        hasUnGrappled = true;
        grappledText.enabled = false;
        grappleLayer = 9;
        walkingSpeed = 1;
        timeSinceLastWin = 0f;
    }




    // Update is called once per frame
    void Update()
    {


        deltaT = Time.time - oldTime;
        oldTime = Time.time;
        timeSinceLastWin += deltaT;


    
        //WASD and jumping movement
        vecForceToAdd *= 0;

        if (jumpCounter < 30)
            jumpCounter++;

        if (isGrounded)
        {

            if (Input.GetKey("space") && jumpCounter >= 30)
            {
                body.AddForce(new Vector3(0, 800, 0));
                jumpCounter = 0;
            }
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
            if ((Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d")))
            {

            //arm/leg swinging
                if (Mathf.Sqrt(rightLeg.transform.rotation.x * rightLeg.transform.rotation.x + rightLeg.transform.rotation.z * rightLeg.transform.rotation.z) > .3)
                    ArmsBackForth *= -1;
                rightLeg.transform.Rotate(new Vector3(3 * ArmsBackForth, 0, 0));
                leftLeg.transform.Rotate(new Vector3(-3 * ArmsBackForth, 0, 0));
                rightArm.transform.Rotate(new Vector3(-3 * ArmsBackForth, 0, 0));
                leftArm.transform.Rotate(new Vector3(3 * ArmsBackForth, 0, 0));

                if (body.velocity.magnitude < 30)
                {
                    vecForceToAdd = vecForceToAdd.normalized;
                    body.AddForce((vecForceToAdd * 20 * walkingSpeed));
                }
            }                   
        





        //this initiates a grapple
            if (Input.GetMouseButtonDown(4))
            {                               
                if (Physics.Raycast(body.transform.position, cam.transform.forward, out hit, 300) && hasUnGrappled && hit.transform.gameObject.layer == grappleLayer)
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
                Vector3 likelyNextPosition = (body.position + (body.velocity) * deltaT);
                likelyNextPosition = hit.point - likelyNextPosition;
                body.AddForce(likelyNextPosition * .8f);                  
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
            walkingSpeed = 1;
        }

		if (collision.gameObject.layer == 11 && timeSinceLastWin > .5f) {
            timeSinceLastWin = 0;
            //tartworldmanager.ChangeWorld();
           //worldmanager.nextLevel();
            Debug.Log("reached win");
            Destroy(currentLevel);
            currentLevel = Instantiate(levels[currentLevelNumber + 1]);
            body.velocity.Set(0, 0, 0);
            body.inertiaTensorRotation.Set(0, 0, 0, 0);
            transform.position = (startingPositions[currentLevelNumber + 1]);
            currentLevelNumber++;
           

		}
        if (collision.gameObject.layer == 10)
        {
            body.velocity.Set(0, 0, 0);
            transform.position = (startingPositions[currentLevelNumber]);
        }

    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {

            isGrounded = false;
            walkingSpeed = .2f;

        }




    }

    IEnumerator KeepUsMoving()
    {

        
        while (true)
        {

            Debug.Log("Coroutine is running");
            
                if (((Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d"))) && body.velocity.magnitude < .05)
                {
                    transform.Translate(0, 1, 0);
                }
            

            yield return new WaitForSeconds(3f);
        }
    }


}
