using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [Header("Main Properties")]
    public int scoreValue = 20;
    public float maxLifeTime = 12f; 

    [Header("Fragments")]
    public bool isFragment = false; 
    public int fragmentCount = 2;
    public float fragmentSpeedMultiplier = 1.5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        StartCoroutine(DeactivateAfterTime());
    }
    
    public void Initialize(Vector2 velocity, float angularVelocity)
    {
        rb.velocity = velocity;
        rb.angularVelocity = angularVelocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.CompareTag("PlayerBullet"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scoreValue);
            }
            other.gameObject.SetActive(false);
            if (!isFragment)
            {
                CreateFragments();
            }
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartGame();
            }
        }
    }

    void CreateFragments()
    {
        for (int i = 0; i < fragmentCount; i++)
        {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector2 velocity = direction * (rb.velocity.magnitude * fragmentSpeedMultiplier);
            float angular = rb.angularVelocity * 2f;
            GameObject fragmentObj = ObjectPooler.Instance.SpawnFromPool("AsteroidFragment", transform.position, transform.rotation);
            if (fragmentObj != null)
            {
                fragmentObj.GetComponent<Asteroid>().Initialize(velocity, angular);
            }
        }
    }

    IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(maxLifeTime);
        gameObject.SetActive(false);
    }
}