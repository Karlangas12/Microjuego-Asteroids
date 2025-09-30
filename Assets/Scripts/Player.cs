using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float thrustForce = 5f;        
    public float rotationSpeed = 180f;   

    Rigidbody2D rb;

    float inputThrust = 0f;
    float inputRotate = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputThrust = Input.GetAxis("Vertical");   
        inputRotate = Input.GetAxis("Horizontal"); 
        float rot = -inputRotate * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, rot);
    }

    void FixedUpdate()
    {
        Vector2 thrustDirection = transform.right;
        Vector2 force = thrustDirection * (inputThrust * thrustForce);
        rb.AddForce(force);
    }
}

