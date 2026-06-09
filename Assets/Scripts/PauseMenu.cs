using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isPaused = false;
    private float savedMusicVolume;
    private float savedSFXVolume;

    private void Start()
    {
        savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", -20f);
        savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", -20f);
        audioMixer.SetFloat("MusicVolume", savedMusicVolume);
        audioMixer.SetFloat("SFXVolume", savedSFXVolume);
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            print("Tab pressed");
            if (optionsMenuUI.activeSelf)
                CloseOptions();
            else
                TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OpenOptions()
    {
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void CloseOptions()
    {
        savedMusicVolume = musicSlider.value;
        savedSFXVolume = sfxSlider.value;
        SaveVolume();
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        if (GameManager.instance != null)
        {
            GameManager.instance.CleanUpAndDestroy();
        }
        SceneManager.LoadScene(0);
    }

    public void UpdateMusicVolume(float volume)
{
    bool result = audioMixer.SetFloat("MusicVolume", volume);
    savedMusicVolume = volume;
}

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        savedSFXVolume = volume;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", savedMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", savedSFXVolume);
    }
}