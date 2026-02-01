using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool startTrigger = false;
    public bool activated = false;
    public GameObject timer;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !activated)
        {
            timer.GetComponent<TimerScript>().SetStartStage();
            activated = true;
            if (!startTrigger)
            {
                player.GetComponent<PlayerScript>().SetGameOver();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
