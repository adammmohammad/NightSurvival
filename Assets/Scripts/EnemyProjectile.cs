using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float damageAmount = 15f;
    public Rigidbody theRB;
    public GameObject impactEffect, damageEffect;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Vector3 velocity = transform.forward * moveSpeed;
        float distance = velocity.magnitude * Time.deltaTime;

        Debug.DrawRay(transform.position, transform.forward * (distance + 0.5f), Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance + 0.5f))
        {
            Debug.Log("Raycast hit: " + hit.transform.name + " Tag: " + hit.transform.tag + " Layer: " + hit.transform.gameObject.layer);
            if (hit.transform.tag == "Player")
            {
                Instantiate(damageEffect, transform.position, Quaternion.identity);
                PlayerHealthController.instance.TakeDamage(damageAmount);
            }
            else if (hit.transform.tag != "Enemy")
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

        theRB.linearVelocity = velocity;
    }
}