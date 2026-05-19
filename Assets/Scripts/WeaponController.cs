using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public AudioClip reloadSound;
    public AudioSource audioSource;
    public float range;
    public Transform cam;
    public LayerMask validLayer;
    public GameObject impactEffect, damageEffect;
    public bool canAutoFire;
    public float timetBtwShoots;
    private float shotsCounter = .01f;
    public int currentAmmo = 100;
    public int clipSize = 15;
    public int remainingAmmo = 300;
    public float damageAmount = 15f;
    public int ammoPickupAmount = 30;
    private UIController uIController;

    void Start()
    {
        
        uIController = Object.FindFirstObjectByType<UIController>();
        // initialize ammo without playing sound
        InitAmmo();
    }

    void InitAmmo()
    {
        remainingAmmo += currentAmmo;
        if (remainingAmmo >= clipSize)
        {
            currentAmmo = clipSize;
            remainingAmmo -= clipSize;
        }
        else
        {
            currentAmmo = remainingAmmo;
            remainingAmmo = 0;
        }
        uIController.updateAmmoText(currentAmmo, remainingAmmo);
    }

    void Update() { }

public void Shoot()
{
    if (GameManager.instance != null && GameManager.instance.isGameOver)
        return;

    if (currentAmmo > 0)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.forward, out hit, range, validLayer))
        {
            if (hit.transform.tag == "Enemy")
            {
                Instantiate(damageEffect, hit.point, Quaternion.identity);
                hit.transform.GetComponentInParent<EnemyController>().TakeDamage(damageAmount);
            }
            else
            {
                Instantiate(impactEffect, hit.point, Quaternion.identity);
            }
        }

        shotsCounter = timetBtwShoots;
        currentAmmo--;
        uIController.updateAmmoText(currentAmmo, remainingAmmo);
    }
}

public void ShootHeld()
{
    if (GameManager.instance != null && GameManager.instance.isGameOver)
        return;

    if (canAutoFire)
    {
        shotsCounter -= Time.deltaTime;
        if (shotsCounter <= 0)
        {
            Shoot();
        }
    }
}

public void Reload()
{
    if (GameManager.instance != null && GameManager.instance.isGameOver)
        return;

    remainingAmmo += currentAmmo;

    if (remainingAmmo >= clipSize)
    {
        currentAmmo = clipSize;
        remainingAmmo -= clipSize;
    }
    else
    {
        currentAmmo = remainingAmmo;
        remainingAmmo = 0;
    }

    uIController.updateAmmoText(currentAmmo, remainingAmmo);

    if (reloadSound != null)
        audioSource.PlayOneShot(reloadSound);
}

    public void GetAmmo()
    {
        remainingAmmo += ammoPickupAmount;
        uIController.updateAmmoText(currentAmmo, remainingAmmo);
    }
}