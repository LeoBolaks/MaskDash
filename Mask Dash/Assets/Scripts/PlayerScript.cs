using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public InputActionReference move;
    public InputActionReference jump;

    public InputActionReference dim1;
    public InputActionReference dim2;
    public InputActionReference dim3;

    public Rigidbody rigidBody;

    private Ray ray;
    private float maxDistanceRay = 10.0f;
    public LayerMask layersToHit;
    private float rayDistance = 0.0f;

    private float speed = 30.0f;
    private float maxSpeed = 50.0f;

    private Vector3 vel;

    private int dimension = 2;

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
        ChangeDimension();

    }

    void ChangeDimension()
    {
        switch(dimension)
        {
            case 1:
                {
                    if (dim2.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 870.0f);
                        dimension = 2;
                    }
                    if (dim3.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (870.0f * 2));
                        dimension = 3;
                    }
                    break;
                }
            case 2:
                {
                    if (dim1.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 870.0f);
                        dimension = 1;
                    }
                    if (dim3.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 870.0f);
                        dimension = 3;
                    }
                    break;
                }
            case 3:
                {
                    if (dim1.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (870.0f * 2));
                        dimension = 1;
                    }
                    if (dim2.action.WasPressedThisFrame())
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 870.0f);
                        dimension = 2;
                    }
                    break;
                }
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
        Debug.Log("Forward Velocity: " + rigidBody.linearVelocity.x);
        rigidBody.AddForce(vel * speed);

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
    }
}
