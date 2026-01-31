using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float JumpForce;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered collider");

        other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, JumpForce, 0));
    }
}
