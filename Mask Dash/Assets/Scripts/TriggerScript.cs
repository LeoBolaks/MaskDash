using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool startTrigger = false;
    public GameObject timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            timer.GetComponent<TimerScript>().SetStartStage();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
