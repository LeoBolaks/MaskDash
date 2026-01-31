using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public InputActionReference move;
    public InputActionReference jump;

    public Rigidbody rigidBody;

    private Ray ray;
    private float maxDistanceRay = 10.0f;
    public LayerMask layersToHit;
    private float rayDistance = 0.0f;

    private float speed = 40.0f;

    private Vector3 vel;

    private bool canJump = true;
    private bool airborne = false;

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

        rigidBody.AddForce(vel * speed);

       
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
            Debug.Log("ray Distance " + rayDistance);
            Debug.DrawLine(hit.point, new Vector3(hit.point.x + 10, hit.point.y, hit.point.z), Color.blue);
        }
    }
}
