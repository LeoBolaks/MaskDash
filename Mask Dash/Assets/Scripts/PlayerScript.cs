using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public InputActionReference move;

    public Rigidbody rigidBody;

    private float speed = 40.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 vel = new Vector3(move.action.ReadValue<Vector2>().y, 0.0f, move.action.ReadValue<Vector2>().x * -1);

        rigidBody.AddForce(vel * speed);

        Debug.Log(rigidBody.linearVelocity.x + ", " + rigidBody.linearVelocity.y + ", " + vel.z);
    }
}
