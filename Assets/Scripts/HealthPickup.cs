using UnityEngine;

public class HealthPickup : MonoBehaviour
{
   public float healAmount = 25f;
public AudioClip pickupSound;

void OnTriggerEnter(Collider other)
{
    if (other.tag == "Player")
    {
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        PlayerHealthController.instance.Heal(healAmount);
        Destroy(gameObject);
    }
}
    void OnTriggerStay(Collider other)
{
    Debug.Log("Something is in trigger: " + other.name + " Tag: " + other.tag);
}
}