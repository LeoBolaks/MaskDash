using UnityEngine;

public class cubeDetailMovement : MonoBehaviour
{
    private float speed = 20.0f;
    private float speedDown = 0.1f;
    private bool playerSlowed = false;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerSlowed = player.GetComponent<PlayerScript>().GetChangingDimension();
    }

    private void FixedUpdate()
    {
        if (!playerSlowed)
        {
            transform.position += new Vector3(-speed, 0, 0);
        }
        else
        {
            transform.position += new Vector3(-speed * speedDown, 0, 0);
        }

        if (transform.position.x < -4000)
        {
            transform.position = new Vector3(4000, transform.position.y, transform.position.z);
        }
    }
}
