using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public Camera camera;

    public InputActionReference move;
    public InputActionReference jump;

    public InputActionReference changeDimension;
    public InputActionReference dimUp;
    public InputActionReference dimDown;


    public Rigidbody rigidBody;

    private Ray ray;
    private float maxDistanceRay = 10.0f;
    public LayerMask layersToHit;
    private float rayDistance = 0.0f;

    private float speed = 30.0f;
    private float speedDown = 0.1f;
   
    private float maxSpeed = 50.0f;

    private Vector3 vel;

    private int dimension = 2;

    private float zoomedFOV = 50.0f;
    private float normalFOV = 90.0f;

    [SerializeField]private bool canJump = true;
    [SerializeField]private bool airborne = false;

    private bool changingDimension = false;
    private bool actionAvailable = true;
    private bool stageStarted = false;
    private bool gameOver = false;

    private Vector3 savedVelocity;

    IEnumerator PauseActionCoRoutine()
    {
        actionAvailable = false;
        yield return new WaitForSeconds(0.2f);
        actionAvailable = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RayCollisionWithGround();

        if (jump.action.WasPressedThisFrame() && canJump)
        {
            rigidBody.AddForce(0.0f, 500.0f, 0.0f);
        }
        if (rayDistance < 1.2f)
        {
            canJump = true;
            airborne = false;
        }
        else
        {
            canJump = false;
            airborne = true;
        }
        if (actionAvailable && changingDimension)
        {
            ChangeDimension();
        }
        if (changeDimension.action.WasPressedThisFrame() && actionAvailable)
        {
            TimeStop();
        }
        ChangeFOV();
    }

    public void StartStage()
    {
        stageStarted = true;
    }

    void ChangeDimension()
    {
        switch(dimension)
        {
            case 1:
                {
                    if (dimUp.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 870.0f);
                        dimension = 2;
                        StartCoroutine(PauseActionCoRoutine());
                    }
                    if (dimDown.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (870.0f * 2));
                        dimension = 3;
                        StartCoroutine(PauseActionCoRoutine());
                    }
                    break;
                }
            case 2:
                {
                    if (dimDown.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 870.0f);
                        dimension = 1;
                        StartCoroutine(PauseActionCoRoutine());
                    }
                    if (dimUp.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 870.0f);
                        dimension = 3;
                        StartCoroutine(PauseActionCoRoutine());
                    }
                    break;
                }
            case 3:
                {
                    if (dimUp.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (870.0f * 2));
                        dimension = 1;
                        StartCoroutine(PauseActionCoRoutine());
                    }
                    if (dimDown.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 870.0f);
                        dimension = 2;
                        StartCoroutine(PauseActionCoRoutine());
                    }
                    break;
                }
        }
        
    }

    void ChangeFOV()
    {
        if (changingDimension)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomedFOV, 0.8f * Time.deltaTime);
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normalFOV, 1.0f * Time.deltaTime);
        }
    }

    void TimeStop()
    {
        if (!changingDimension)
        {
            savedVelocity = rigidBody.linearVelocity;
            //rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x * speedDown, rigidBody.linearVelocity.y * speedDown, rigidBody.linearVelocity.z * speedDown);
            rigidBody.useGravity = false;
            changingDimension = true;
            StartCoroutine(PauseActionCoRoutine());

        }
        else
        {
            //rigidBody.constraints = RigidbodyConstraints.None;
            //rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidBody.linearVelocity = savedVelocity;
            rigidBody.useGravity = true;
            changingDimension = false;
            StartCoroutine(PauseActionCoRoutine());

        }
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(ray.origin, ray.direction * maxDistanceRay, Color.red);
        if (!airborne)
        {
            vel = new Vector3(move.action.ReadValue<Vector2>().y, 0.0f, move.action.ReadValue<Vector2>().x * -1);
        }
        else 
        {
            vel = new Vector3(0.0f , 0.0f, move.action.ReadValue<Vector2>().x * -1);
        }
        if (!changingDimension)
        {
            rigidBody.AddForce(vel * speed);
        }
        else
        {
            rigidBody.AddForce((vel * speed) * speedDown);
            rigidBody.AddForce(0.0f, -0.1f, 0.0f);

        }
            Vector3 v = rigidBody.linearVelocity;
        v.x = Mathf.Clamp(v.x, -maxSpeed, maxSpeed);
        rigidBody.linearVelocity = v;
       

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    void RayCollisionWithGround()
    {
        ray = new Ray(rigidBody.transform.position, -rigidBody.transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceRay, layersToHit))
        {
            rayDistance = Vector3.Distance(rigidBody.position, hit.point);
            Debug.DrawLine(hit.point, new Vector3(hit.point.x + 10, hit.point.y, hit.point.z), Color.blue);
        }
        else
        {
            rayDistance = 999.0f;
        }
    }
}
