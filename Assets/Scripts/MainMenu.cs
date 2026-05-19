using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public AudioClip menuMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
    }

    public void ShowMenu()
    {
        mainMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // disable player input
        PlayerController.instance.enabled = false;
        WeaponController wc = FindObjectOfType<WeaponController>();
        if (wc != null) wc.enabled = false;

        if (menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        if (BackgroundMusic.instance != null)
            BackgroundMusic.instance.StopMusic();
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // re-enable player input
        PlayerController.instance.enabled = true;
        WeaponController wc = FindObjectOfType<WeaponController>();
        if (wc != null) wc.enabled = true;

        audioSource.Stop();

        if (BackgroundMusic.instance != null)
            BackgroundMusic.instance.StartMusic();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}