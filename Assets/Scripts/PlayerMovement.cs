using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 180f;
    public float lerpRate = 5f;
    public float baseSpeed = 10f;

    private Rigidbody2D rb;
    private float currentSpeed;
    private float turnInput;
    private float speedInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        turnInput = 0f;
        speedInput = 0f;

        if (Input.GetKey(KeyCode.A)) turnInput = 1f;
        if (Input.GetKey(KeyCode.D)) turnInput = -1f;

        if (Input.GetKey(KeyCode.W)) speedInput = 1f;
        else if (Input.GetKey(KeyCode.S)) speedInput = -1f;
    }

    void FixedUpdate()
    {
        float rotationAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        float targetSpeed = baseSpeed;
        if (speedInput > 0) targetSpeed = baseSpeed;
        else if (speedInput < 0) targetSpeed = baseSpeed * 0.5f;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpRate * Time.fixedDeltaTime);

        Vector2 forward = transform.up;
        rb.velocity = forward * currentSpeed;
    }
}
