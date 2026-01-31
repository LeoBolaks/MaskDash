using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public static bool m_gamePaused = false;

    public GameObject m_pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (m_gamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    void Resume()
    {
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        m_gamePaused = false;
    }
    void Pause()
    {
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        m_gamePaused = true;
    }



}
