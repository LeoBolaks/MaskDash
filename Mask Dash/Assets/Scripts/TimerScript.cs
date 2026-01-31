using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_timerText;
    [SerializeField] float m_timeRemaining;
    private bool stageStarted = false;
    private void Update()
    {
        if (stageStarted)
        {
            m_timeRemaining += Time.deltaTime;
        }
       

        //int seconds = Mathf.FloorToInt(m_timeRemaining % 60);
        m_timerText.text = string.Format("{00:00.00}", m_timeRemaining);
    }
    public void SetStartStage()
    {
        stageStarted = !stageStarted;
    }
}
