using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject optionsMenu;
    public GameObject mainMenu;
    public Button continueButton;
    private float savedMusicVolume;
    private float savedSFXVolume;

    private void Start()
    {
        savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", -20f);
        savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", -20f);
        audioMixer.SetFloat("MusicVolume", savedMusicVolume);
        audioMixer.SetFloat("SFXVolume", savedSFXVolume);
        MusicManager.Instance.PlayMusic("MainMenu");
        continueButton.interactable = SaveManager.Instance != null && SaveManager.Instance.HasSave();
    }
    
    public void OpenOptions()
    {
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseOptions()
    {
        savedMusicVolume = musicSlider.value;
        savedSFXVolume = sfxSlider.value;
        SaveVolume();
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void NewGame()
    {
        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.HasSave())
            {
                File.Delete(SaveManager.Instance.saveFile);
            }
            SaveManager.Instance.data = new GameData();
            SaveManager.Instance.hasLoadedData = false;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SaveManager.Instance.LoadData();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}