using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioClip winSound;
    public bool isGameOver = false;
    private AudioSource audioSource;
    public static GameManager instance;

    public int enemiesKilled;
    public int enemiesToWin = 3;

    public GameObject winScreen;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

public void EnemyKilled()
{
    enemiesKilled++;
    ObjectiveManager.instance.UpdateEnemyCount(enemiesKilled);

    if (enemiesKilled >= enemiesToWin)
    {
        ObjectiveManager.instance.StopTimer();
        WinGame();
    }
}

    void WinGame()
    {
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      //  Time.timeScale = 0f;
        BackgroundMusic.instance.StopMusic();

        if (winSound != null)
            audioSource.PlayOneShot(winSound);
    }
}
