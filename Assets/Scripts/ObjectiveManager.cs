using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public TMP_Text objectiveText;
    public TMP_Text enemyCountText;
    public TMP_Text timerText;

    public float timeUntilSunrise = 180f; // 3 minutes
    private float currentTime;
    private bool gameActive = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentTime = timeUntilSunrise;
        gameActive = true;
        objectiveText.text = "Objective: Kill 20 enemies before sunrise!";
        UpdateEnemyCount(0);
    }

    void Update()
    {
        if (!gameActive || Time.timeScale == 0f) return;

        currentTime -= Time.deltaTime;
      if (currentTime <= 0)
{
    currentTime = 0;
    gameActive = false;
    DayNightCycle dayNight = FindObjectOfType<DayNightCycle>();
    if (dayNight != null)
        dayNight.Sunrise();
    PlayerController.instance.isDead = true;
    UIController.instance.showDeathScreen();
}

        // format time as MM:SS
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("Time Until Sunrise: {0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateEnemyCount(int killed)
    {
        enemyCountText.text = "Enemies Killed: " + killed + " / 20";
    }

    public void StopTimer()
    {
        gameActive = false;
    }
}