using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public float spawnRatePerMinute = 30f; 
    public float spawnRateIncrement = 2f;  
    public float spawnDistance = 1.5f;

    [Header("Asteroid Properties")]
    public float minSpeed = 1.5f;
    public float maxSpeed = 3.5f;
    public float minAngular = -90f;
    public float maxAngular = 90f;

    private Camera mainCam;
    private float nextSpawnTime = 0f;
    private float camHeight, camWidth;
    private Vector3 camPos;

    void Start()
    {
        mainCam = Camera.main;
        UpdateCameraBounds();
    }
    void Update()
    {
        if (Time.time < nextSpawnTime) return;
        nextSpawnTime = Time.time + (60f / spawnRatePerMinute);
        spawnRatePerMinute += spawnRateIncrement * Time.deltaTime;
        SpawnAsteroid();
    }

    void SpawnAsteroid()
    {
        UpdateCameraBounds();

        int side = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        float left = camPos.x - camWidth / 2f;
        float right = camPos.x + camWidth / 2f;
        float bottom = camPos.y - camHeight / 2f;
        float top = camPos.y + camHeight / 2f;

        switch (side)
        {
            case 0: spawnPos = new Vector2(Random.Range(left, right), top + spawnDistance); break;
            case 1: spawnPos = new Vector2(Random.Range(left, right), bottom - spawnDistance); break;
            case 2: spawnPos = new Vector2(left - spawnDistance, Random.Range(bottom, top)); break;
            default: spawnPos = new Vector2(right + spawnDistance, Random.Range(bottom, top)); break;
        }

        Vector2 targetDir = (new Vector2(Random.Range(left, right), Random.Range(bottom, top)) - spawnPos).normalized;
        float speed = Random.Range(minSpeed, maxSpeed);
        float angular = Random.Range(minAngular, maxAngular);

        GameObject asteroidObj = ObjectPooler.Instance.SpawnFromPool("Asteroid", spawnPos, Quaternion.identity);
        
        if (asteroidObj != null)
        {
            Asteroid ast = asteroidObj.GetComponent<Asteroid>();
            if (ast != null)
            {
                ast.Initialize(targetDir * speed, angular);
            }
        }
    }

    void UpdateCameraBounds()
    {
        camHeight = 2f * mainCam.orthographicSize;
        camWidth = camHeight * mainCam.aspect;
        camPos = mainCam.transform.position;
    }
}