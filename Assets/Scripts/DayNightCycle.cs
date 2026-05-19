using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;

    public float dayIntensity = 1f;
    public float nightIntensity = 0.2f;

    void Start()
    {
        // start at night
        sun.intensity = nightIntensity;
    }

    public void Sunrise()
    {
        // called when timer runs out
        sun.intensity = dayIntensity;
        Debug.Log("Sunrise - Game Over!");
    }
}