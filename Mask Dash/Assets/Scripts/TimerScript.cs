using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_timerText;
    [SerializeField] float m_timeRemaining;
    private void Update()
    {
        if (m_timeRemaining > 0)
        {
            m_timeRemaining -= Time.deltaTime;
        }
        else
        {
            m_timeRemaining = 0;
            Debug.Log("Game Over");
        }

        int seconds = Mathf.FloorToInt(m_timeRemaining % 60);
        m_timerText.text = string.Format("{00:00}", seconds);
    }
}
