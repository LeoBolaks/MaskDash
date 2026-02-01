using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public Camera camera;

    public GameObject maskStartPoint;
    public GameObject maskEndPoint;
    public GameObject maskMovePoint;
    public GameObject greenMask;
    public GameObject redMask;
    public GameObject blueMask;

    public GameObject startPointMap;

    public InputActionReference move;
    public InputActionReference jump;

    public InputActionReference returnToMenu;

    public InputActionReference changeDimension;
    public InputActionReference dimUp;
    public InputActionReference dimDown;

    private MeshRenderer[] childRenderersGreen;
    private MeshRenderer[] childRenderersRed;
    private MeshRenderer[] childRenderersBlue;

    AnimatorStateInfo greenMaskState;


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
    private float normalFOV = 100.0f;

    [SerializeField]private bool canJump = true;
    [SerializeField]private bool airborne = false;

    private bool changingDimension = false;
    private bool actionAvailable = true;
    private bool canSwapDims = true;
    private bool stageStarted = false;
    private bool gameOver = false;

    private bool maskOut = false;
    private float distanceToMaskEndPoint = 0.0f;

    private Vector3 savedVelocity;

    IEnumerator PauseActionCoRoutine()
    {
        actionAvailable = false;
        yield return new WaitForSeconds(0.8f);
        actionAvailable = true;
    }

    IEnumerator PauseSwappingDimsCoRoutine()
    {
        canSwapDims = false;
        yield return new WaitForSeconds(0.3f);
        canSwapDims = true;
    }

    IEnumerator BackToMenuCoRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator animateMaskCoRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        greenMask.GetComponent<Animator>().SetTrigger("Slowed");
        blueMask.GetComponent<Animator>().SetTrigger("Slowed");
        redMask.GetComponent<Animator>().SetTrigger("Slowed");
        maskOut = true;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childRenderersGreen = greenMask.GetComponentsInChildren<MeshRenderer>();
        childRenderersBlue = blueMask.GetComponentsInChildren<MeshRenderer>();
        childRenderersRed = redMask.GetComponentsInChildren<MeshRenderer>();
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
        if (changingDimension && canSwapDims && maskOut)
        {
            ChangeDimension();
        }
        if (changeDimension.action.WasPressedThisFrame() && actionAvailable)
        {
            if (maskOut)
            {
                greenMask.GetComponent<Animator>().SetTrigger("Resumed");
                blueMask.GetComponent<Animator>().SetTrigger("Resumed");
                redMask.GetComponent<Animator>().SetTrigger("Resumed");
                maskOut = false;
            }
            else
            {
                StartCoroutine(animateMaskCoRoutine());
            }
            TimeStop();
        }
        ChangeFOV();
        if (returnToMenu.action.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Main Menu");
        }
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
                        StartCoroutine(PauseSwappingDimsCoRoutine());
                    }
                    if (dimDown.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (870.0f * 2));
                        dimension = 3;
                        StartCoroutine(PauseActionCoRoutine());
                        StartCoroutine(PauseSwappingDimsCoRoutine());
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
                        StartCoroutine(PauseSwappingDimsCoRoutine());
                    }
                    if (dimUp.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 870.0f);
                        dimension = 3;
                        StartCoroutine(PauseActionCoRoutine());
                        StartCoroutine(PauseSwappingDimsCoRoutine());
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
                        StartCoroutine(PauseSwappingDimsCoRoutine());
                    }
                    if (dimDown.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 870.0f);
                        dimension = 2;
                        StartCoroutine(PauseActionCoRoutine());
                        StartCoroutine(PauseSwappingDimsCoRoutine());
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

    public void SetGameOver()
    {
        gameOver = true;
        StartCoroutine(BackToMenuCoRoutine());
    }

    void TimeStop()
    {
        if (!changingDimension)
        {
            savedVelocity.x = rigidBody.linearVelocity.x;
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
            rigidBody.linearVelocity = new Vector3(savedVelocity.x, rigidBody.linearVelocity.y, rigidBody.linearVelocity.z);
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
            rigidBody.AddForce(0.0f, -0.05f, 0.0f);

        }
            Vector3 v = rigidBody.linearVelocity;
        v.x = Mathf.Clamp(v.x, -maxSpeed, maxSpeed);
        rigidBody.linearVelocity = v;

        greenMask.transform.position = maskMovePoint.transform.position;
        redMask.transform.position = maskMovePoint.transform.position;
        blueMask.transform.position = maskMovePoint.transform.position;

        switch (dimension)
        {
            case 1:
                {
                    if (!maskOut)
                    {
                        foreach (MeshRenderer rend in childRenderersBlue)
                        {
                            rend.enabled = false;
                        }
                        foreach (MeshRenderer rend in childRenderersRed)
                        {
                            rend.enabled = true;
                        }
                        foreach (MeshRenderer rend in childRenderersGreen)
                        {
                            rend.enabled = false;
                        }
                    }
                    if (distanceToMaskEndPoint < 1.0f)
                    {
                        foreach (MeshRenderer rend in childRenderersRed)
                        {
                            rend.enabled = false;
                        }
                        maskOut = true;
                    }
                    else
                    {
                        foreach (MeshRenderer rend in childRenderersRed)
                        {
                            rend.enabled = true;
                        }
                        maskOut = false;
                    }
                    break;
                }
            case 2:
                {
                    if (!maskOut)
                    {
                        foreach (MeshRenderer rend in childRenderersGreen)
                        {
                            rend.enabled = true;
                        }
                        foreach (MeshRenderer rend in childRenderersRed)
                        {
                            rend.enabled = false;
                        }
                        foreach (MeshRenderer rend in childRenderersBlue)
                        {
                            rend.enabled = false;
                        }
                    }
                    if (distanceToMaskEndPoint < 1.0f)
                    {
                        foreach (MeshRenderer rend in childRenderersGreen)
                        {
                            rend.enabled = false;
                        }
                        maskOut = true;
                    }
                    else
                    {
                        foreach (MeshRenderer rend in childRenderersGreen)
                        {
                            rend.enabled = true;
                        }
                        maskOut = false;
                    }
                    break;
                }
            case 3:
                {
                    if (!maskOut)
                    {
                        foreach (MeshRenderer rend in childRenderersRed)
                        {
                            rend.enabled = false;
                        }
                        foreach (MeshRenderer rend in childRenderersGreen)
                        {
                            rend.enabled = false;
                        }
                        foreach (MeshRenderer rend in childRenderersBlue)
                        {
                            rend.enabled = true;
                        }
                    }
                    if (distanceToMaskEndPoint < 1.0f)
                    {
                        foreach (MeshRenderer rend in childRenderersBlue)
                        {
                            rend.enabled = false;
                        }
                        maskOut = true;
                    }
                    else
                    {
                        foreach (MeshRenderer rend in childRenderersBlue)
                        {
                            rend.enabled = true;
                        }
                        maskOut = false;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        distanceToMaskEndPoint = Vector3.Distance(maskMovePoint.transform.position, maskEndPoint.transform.position);
        

        if (changingDimension)
        {
            maskMovePoint.transform.position = Vector3.Lerp(maskMovePoint.transform.position, maskEndPoint.transform.position, 3.0f * Time.deltaTime);
        }
        else
        {
            maskMovePoint.transform.position = Vector3.Lerp(maskMovePoint.transform.position, maskStartPoint.transform.position, 3.0f * Time.deltaTime);
        }    

    }

    public bool GetChangingDimension()
    {
        return changingDimension;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
        {
            SceneManager.LoadScene("Main Menu");
        }
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
