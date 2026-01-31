using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Slider m_volumeSlider;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Options()
    {
        //TextMeshPro.GetComponent<>
    }

    public void ChangeVolume()
    {
        AudioListener.volume = m_volumeSlider.value;
        Save();
    }

    void Load()
    {
        m_volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", m_volumeSlider.value);
    }

    public void Exit()
    {
        Debug.Log("Exit button pressed");
    }

}
