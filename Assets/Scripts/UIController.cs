using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public void Awake()
    {
        instance = this;
    }

    public TMP_Text currentAmmoText;
    public TMP_Text remainingAmmoText;
    public GameObject deathScreen;

    public AudioClip gameOverSound;
    public AudioClip lossLoopSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() { }

    public void updateAmmoText(int currentAmmo, int remainingAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString();
        remainingAmmoText.text = "/" + remainingAmmo;
    }

    public void showDeathScreen()
    {
        GameManager.instance.isGameOver = true;

        deathScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (BackgroundMusic.instance != null)
            BackgroundMusic.instance.StopMusic();

        StartCoroutine(PlayDeathSounds());
    }

    IEnumerator PlayDeathSounds()
    {
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
            yield return new WaitForSeconds(gameOverSound.length);
        }
        // then loop loss music
        if (lossLoopSound != null)
        {
            audioSource.clip = lossLoopSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void showWinScreen()
    {
        if (BackgroundMusic.instance != null)
            BackgroundMusic.instance.StopMusic();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Gamee");
    }
        public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}