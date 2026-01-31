using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] float BoostForce;
    [SerializeField] float JumpForce;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered collider");

        other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(BoostForce, JumpForce, 0));
    }
}
