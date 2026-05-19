using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public AudioClip deathSound;
    private AudioSource audioSource;
    public static PlayerHealthController instance;
    public void Awake()
    {
        instance = this;
    }
    public float maxHealth = 100f;
    private float currentHealth;
    public Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarFill.fillAmount = 1f;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBarFill.fillAmount = currentHealth / maxHealth;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            if (deathSound != null)
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            currentHealth = 0;
            healthBarFill.fillAmount = 0f;
            PlayerController.instance.isDead = true;
            UIController.instance.showDeathScreen();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}