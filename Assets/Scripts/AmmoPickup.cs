using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
public AudioClip pickupSound;

void OnTriggerEnter(Collider other)
{
    if (other.tag == "Player")
    {
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Object.FindFirstObjectByType<WeaponController>().GetAmmo();
        Destroy(gameObject);
    }
}
}