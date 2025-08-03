using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] List<Vector2> bounds;
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

        if (Input.GetKey(KeyCode.S)) speedInput = -1f;
        else speedInput = 0;
    }

    void FixedUpdate()
    {
        CollisionAvoidance(bounds);

        float rotationAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        float targetSpeed = baseSpeed;
        if (speedInput >= 0) targetSpeed = baseSpeed;
        else if (speedInput < 0) targetSpeed = baseSpeed * 0.5f;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpRate * Time.fixedDeltaTime);

        Vector2 forward = transform.up;
        rb.velocity = forward * currentSpeed;
    }


    Vector2 debugLRay;
    Vector2 debugCRay;
    Vector2 debugRRay;
    bool debugLhit;
    bool debugChit;
    bool debugRhit;
    // bounds check
    void CollisionAvoidance(List<Vector2> collider)
    {
        Vector2 leftQ = transform.rotation * new Vector2(-1f, 4);
        Vector2 centerQ = transform.rotation * new Vector2(0, 6);
        Vector2 rightQ = transform.rotation * new Vector2(1f, 4);
        leftQ += (Vector2)transform.position;
        rightQ += (Vector2)transform.position;
        centerQ += (Vector2)transform.position;
        debugLRay = leftQ;
        debugCRay = centerQ;
        debugRRay = rightQ;

        float L = 0;
        float R = 0;
        float C = 0;
        for(int i = 0; i < collider.Count - 1; ++i)
        {
            float temp = LineIntersection(transform.position, leftQ, collider[i], collider[i + 1]);
            if (temp > L) { L = temp; }
            temp = LineIntersection(transform.position, rightQ, collider[i], collider[i + 1]);
            if (temp > R) { R = temp; }
            temp = LineIntersection(transform.position, centerQ, collider[i], collider[i + 1]);
            if (temp > C) { C = temp; }
        }
        debugRhit = R != 0;
        debugLhit = L != 0;
        debugChit = C != 0 && R != 0 && L != 0;
        if (R == 0 && L == 0) {return;}
        speedInput = -1;
        if(R > 0 && L > 0)
        {
            if (C > 0)
            {
                turnInput = 2.5f;
                return;
            }
        }
        if(R >= L)
        {
            turnInput = R*2 + 0.5f;
        }
        else
        {
            turnInput = -(L*2 + 0.5f);
        }
    }

    float LineIntersection(Vector2 QueryOrigin, Vector2 QueryEnd, Vector2 LineStart, Vector2 LineEnd)
    {
        Vector2 A = QueryOrigin;
        Vector2 B = QueryEnd;
        Vector2 C = LineStart;
        Vector2 D = LineEnd;
        float denominator = (A.x - B.x) * (C.y - D.y) - (A.y - B.y) * (C.x - D.x);
        if (denominator == 0) return 0;

        float t = ((A.x - C.x) * (C.y - D.y) - (A.y - C.y) * (C.x - D.x)) / denominator;
        float u = ((A.x - C.x) * (A.y - B.y) - (A.y - C.y) * (A.x - B.x)) / denominator;

        if(!(t > 0 && t < 1 && u > 0 && u < 1))
        {
            return 0;
        }
        return 1 - t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugLhit ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, debugLRay);
        Gizmos.color = debugChit ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, debugCRay);
        Gizmos.color = debugRhit ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, debugRRay);

        Gizmos.color = Color.red;
        for (int i = 0; i < bounds.Count - 1; ++i)
        {
            Gizmos.DrawLine(bounds[i], bounds[i+1]);
        }
    }
}
