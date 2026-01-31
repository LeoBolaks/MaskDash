using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider m_volumeSlider;
    [SerializeField] AudioMixer m_masterAudioMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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



}
