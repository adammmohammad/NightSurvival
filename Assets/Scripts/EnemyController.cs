using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class EnemyController : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;
    private PlayerController playerController;
    public float moveSpeed;
    public Rigidbody theRB;
    public float chaseRange = 15f;
    public float stopCloseRange = 4f;
    public Animator anim;
    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    public Transform pointsHolder;
    public float pointWaitTime = 3f;
    public float waitCounter;
    private bool isDead;
    public float maxHealth = 25f;
    private float currentHealth;
    public Image healthBarFill;

    public Transform shootPoint;
    public EnemyProjectile projectile;
    public float timeBtwShots = 2f;
    private float shotCounter;
    public float shootDamage = 10f;
    public GameObject impactEffect;
    public GameObject damageEffect;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();
        currentHealth = maxHealth;

        if (healthBarFill == null)
        {
            Image[] images = GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.name == "HealthBarFill")
                {
                    healthBarFill = img;
                    break;
                }
            }
        }

        if (healthBarFill != null)
            healthBarFill.fillAmount = 1f;

        if (patrolPoints.Length == 0 || patrolPoints[0] == null)
        {
            GameObject pointsHolderObject = GameObject.Find("PatrolPoints");
            if (pointsHolderObject != null)
            {
                pointsHolder = pointsHolderObject.transform;
                patrolPoints = new Transform[pointsHolderObject.transform.childCount];
                for (int i = 0; i < pointsHolderObject.transform.childCount; i++)
                {
                    patrolPoints[i] = pointsHolderObject.transform.GetChild(i);
                }
            }
        }

        if (pointsHolder != null)
            pointsHolder.SetParent(null);

        waitCounter = Random.Range(.75f, 1.25f) * pointWaitTime;
        shotCounter = timeBtwShots;
    }

    void Update()
    {
        if (isDead || Time.timeScale == 0f) return;

        if (healthBarFill != null)
        {
            healthBarFill.transform.parent.LookAt(Camera.main.transform.position);
        }

        float distance = Vector3.Distance(transform.position, playerController.transform.position);
        float yStore = theRB.linearVelocity.y;

        if (distance < chaseRange && !PlayerController.instance.isDead)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBtwShots;
                EnemyShoot();
            }

            transform.LookAt(
                new Vector3(
                    playerController.transform.position.x,
                    transform.position.y,
                    playerController.transform.position.z
                )
            );

            if (distance > stopCloseRange)
            {
                theRB.linearVelocity = transform.forward * moveSpeed;
                anim.SetBool("moving", true);
            }
            else
            {
                theRB.linearVelocity = Vector3.zero;
                anim.SetBool("moving", false);
            }
        }
        else
        {
            if (patrolPoints.Length > 0 && patrolPoints[0] != null)
            {
                if (Vector3.Distance(
                        transform.position,
                        new Vector3(
                            patrolPoints[currentPatrolIndex].position.x,
                            transform.position.y,
                            patrolPoints[currentPatrolIndex].position.z
                        )
                    ) < .25f)
                {
                    waitCounter -= Time.deltaTime;
                    theRB.linearVelocity = Vector3.zero;
                    anim.SetBool("moving", false);
                    if (waitCounter <= 0)
                    {
                        currentPatrolIndex++;
                        if (currentPatrolIndex >= patrolPoints.Length)
                            currentPatrolIndex = 0;
                        waitCounter = Random.Range(.75f, 1.25f) * pointWaitTime;
                    }
                }
                else
                {
                    transform.LookAt(
                        new Vector3(
                            patrolPoints[currentPatrolIndex].position.x,
                            transform.position.y,
                            patrolPoints[currentPatrolIndex].position.z
                        )
                    );
                    theRB.linearVelocity = transform.forward * moveSpeed;
                    anim.SetBool("moving", true);
                }
            }
            else
            {
                theRB.linearVelocity = Vector3.zero;
                anim.SetBool("moving", false);
            }
        }

        theRB.linearVelocity = new Vector3(theRB.linearVelocity.x, yStore, theRB.linearVelocity.z);
    }

    void EnemyShoot()
    {
        if (shootPoint == null) return;
        shootPoint.LookAt(playerController.theCam.transform.position);
        anim.SetTrigger("shooting");
        StartCoroutine(SpawnBulletDelay());
    }

    IEnumerator SpawnBulletDelay()
    {
        yield return new WaitForSeconds(0.3f);

        if (projectile != null)
        {
            EnemyProjectile newProjectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
            newProjectile.damageAmount = shootDamage;
        }
        else
        {
            RaycastHit hit;
            Vector3 shootDirection = (playerController.theCam.transform.position) - shootPoint.position;
            int layerMask = ~(1 << gameObject.layer);

            if (Physics.Raycast(shootPoint.position, shootDirection.normalized, out hit, chaseRange, layerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    if (damageEffect != null)
                        Instantiate(damageEffect, hit.point, Quaternion.identity);
                    PlayerHealthController.instance.TakeDamage(shootDamage);
                }
                else if (hit.transform.tag != "Enemy")
                {
                    if (impactEffect != null)
                        Instantiate(impactEffect, hit.point, Quaternion.identity);
                }
            }
        }
    }

    public void TakeDamage(float damageToTake)
    {
        if (isDead) return;

        currentHealth -= damageToTake;
        if (healthBarFill != null)
            healthBarFill.fillAmount = currentHealth / maxHealth;

        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        GameManager.instance.EnemyKilled();
        Destroy(gameObject);
    }
}