using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab; // Assign the sphere prefab in the inspector
    public Transform spawnPoint; // Set this to a GameObject in the scene as the spawn point
    public int numberOfBalls = 5; // Number of spheres to spawn
    public float speed = 5f; // Speed of each sphere

    private int spawnedCount = 0; // Track number of spawned spheres

    void Start()
    {
        SpawnSpheres();
    }

    void SpawnSpheres()
    {
        for (int i = 0; i < numberOfBalls; i++)
        {
            SpawnSphere();
        }
    }

    void SpawnSphere()
    {
        // Check if spawn point is assigned, otherwise use the spawner's position
        Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;

        // Instantiate the sphere at the spawn point with no rotation
        GameObject sphereInstance = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        
        // Configure Rigidbody component
        Rigidbody sphereRb = sphereInstance.GetComponent<Rigidbody>();
        if (sphereRb == null)
        {
            sphereRb = sphereInstance.AddComponent<Rigidbody>();
        }

        // Rigidbody settings for movement
        sphereRb.useGravity = false; // Disable gravity
       
        sphereRb.collisionDetectionMode = CollisionDetectionMode.Continuous; // For more accurate bouncing

        // Generate a random direction in the XZ plane and set initial velocity
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        sphereRb.velocity = randomDirection * speed;

        // Attach constant velocity script to maintain speed
        sphereInstance.AddComponent<ConstantVelocity>().SetSpeed(speed);
    }
}

public class ConstantVelocity : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Keep the velocity at the specified speed, preserving direction
        rb.velocity = rb.velocity.normalized * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reflect the velocity upon collision to simulate a bounce
        Vector3 reflection = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
        rb.velocity = reflection.normalized * speed;
    }
}
