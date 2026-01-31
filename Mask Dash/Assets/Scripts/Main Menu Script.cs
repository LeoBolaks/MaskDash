using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Options()
    {

    }

    public void Exit()
    {
        Debug.Log("Exit button pressed");
    }

}
