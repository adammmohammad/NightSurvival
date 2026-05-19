using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip menuMusic;
    private AudioSource audioSource;

    public string gameSceneName = "Demo_Scene";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        audioSource = GetComponent<AudioSource>();

        if (menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StartGame()
    {
        audioSource.Stop();
        audioSource.clip = null;
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}